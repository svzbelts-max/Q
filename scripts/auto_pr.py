#!/usr/bin/env python3
"""
Auto PR script:
- Reads BOT_INSTRUCTIONS.md from repo root (required).
- Asks OpenAI to produce a unified diff patch implementing the instruction.
- Applies the patch, commits, pushes a branch and opens a PR.

REQUIREMENTS:
- env OPENAI_API_KEY : OpenAI API key
- env BOT_PAT         : Personal access token with repo scope (or use GitHub App token workflow)
- env GITHUB_REPOSITORY : owner/repo (set by Actions)
"""
import os
import subprocess
import uuid
import requests
import sys
import json
from pathlib import Path

OPENAI_API_KEY = os.getenv("OPENAI_API_KEY")
BOT_PAT = os.getenv("BOT_PAT")
GITHUB_REPOSITORY = os.getenv("GITHUB_REPOSITORY")  # owner/repo

if not OPENAI_API_KEY or not BOT_PAT or not GITHUB_REPOSITORY:
    print("Missing required environment variables. Ensure OPENAI_API_KEY, BOT_PAT and GITHUB_REPOSITORY are set.")
    sys.exit(1)

OWNER, REPO = GITHUB_REPOSITORY.split("/")

ROOT = Path.cwd()
INSTRUCTIONS_FILE = ROOT / "BOT_INSTRUCTIONS.md"
if not INSTRUCTIONS_FILE.exists():
    print("BOT_INSTRUCTIONS.md not found in repo root. Create this file with the task for the bot.")
    sys.exit(1)

with INSTRUCTIONS_FILE.open("r", encoding="utf-8") as f:
    task_text = f.read().strip()

if not task_text:
    print("BOT_INSTRUCTIONS.md is empty. Add a clear instruction of what to change.")
    sys.exit(1)

# Ensure git working directory is clean
res = subprocess.run(["git", "status", "--porcelain"], capture_output=True, text=True)
if res.returncode != 0:
    print("git status failed")
    print(res.stderr)
    sys.exit(1)
if res.stdout.strip():
    print("Working directory not clean; please commit or stash changes before running.")
    sys.exit(1)

# Prepare prompt for OpenAI
system = (
    "You are a helpful assistant that outputs a single unified diff patch (git format) and nothing else. "
    "The repository files are available to be patched by the diff. Produce a valid 'git apply' compatible "
    "unified diff that implements the requested changes. Do not include explanations or extra text."
)
user = (
    f"Task: {task_text}\n\n"
    "Constraints:\n"
    "- Only modify files necessary to accomplish the task.\n"
    "- Output MUST be a unified diff (git-style) starting with lines like 'diff --git a/... b/...'.\n"
    "- If no changes are needed, output an empty response (or a comment with 'NO_CHANGES').\n"
    "- Keep changes minimal and safe; do not remove critical files.\n\n"
    "If the task requires code context that isn't present, attempt conservative safe changes or indicate NO_CHANGES."
)

# Call OpenAI ChatCompletion (compatibility with both old/new SDKs)
def ask_openai(system_prompt: str, user_prompt: str) -> str:
    url = "https://api.openai.com/v1/chat/completions"
    headers = {
        "Authorization": f"Bearer {OPENAI_API_KEY}",
        "Content-Type": "application/json"
    }
    payload = {
        "model": "gpt-4",
        "messages": [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_prompt}
        ],
        "temperature": 0.2,
        "max_tokens": 2000
    }
    resp = requests.post(url, headers=headers, json=payload, timeout=300)
    resp.raise_for_status()
    data = resp.json()
    # Extract assistant content
    return data["choices"][0]["message"]["content"]

print("Requesting patch from OpenAI...")
try:
    patch_text = ask_openai(system, user)
except Exception as e:
    print("OpenAI request failed:", e)
    sys.exit(1)

if not patch_text.strip() or "NO_CHANGES" in patch_text:
    print("No changes suggested by the model.")
    sys.exit(0)

PATCH_FILE = ROOT / "bot_generated.patch"
PATCH_FILE.write_text(patch_text, encoding="utf-8")
print(f"Wrote patch to {PATCH_FILE}")

# Try to apply patch
apply_proc = subprocess.run(["git", "apply", "--index", str(PATCH_FILE)], capture_output=True, text=True)
if apply_proc.returncode != 0:
    print("git apply failed:")
    print(apply_proc.stderr)
    # Dump patch for debugging
    print("--- PATCH CONTENT ---")
    print(patch_text)
    sys.exit(1)

# Create branch, commit, push
branch = f"bot/auto-update-{uuid.uuid4().hex[:8]}"
subprocess.check_call(["git", "checkout", "-b", branch])

# Commit staged changes
commit_msg = "chore: automated changes by bot"
subprocess.check_call(["git", "commit", "-m", commit_msg])

# Push branch using BOT_PAT (already used by actions/checkout for auth) but ensure origin present
push_proc = subprocess.run(["git", "push", "--set-upstream", "origin", branch], capture_output=True, text=True)
if push_proc.returncode != 0:
    print("git push failed:")
    print(push_proc.stderr)
    sys.exit(1)

print(f"Pushed branch {branch}")

# Create PR via GitHub API
pr_title = f"Automated changes: {task_text.splitlines()[0][:80]}"
pr_body = (
    "This Pull Request was created automatically by the repository bot.\n\n"
    "Task:\n```\n" + (task_text if len(task_text) < 1000 else task_text[:1000] + "...") + "\n```\n"
    "If this PR is incorrect, please close it and update BOT_INSTRUCTIONS.md."
)
create_url = f"https://api.github.com/repos/{OWNER}/{REPO}/pulls"
headers = {"Authorization": f"token {BOT_PAT}", "Accept": "application/vnd.github.v3+json"}
payload = {"title": pr_title, "head": branch, "base": "main", "body": pr_body}
r = requests.post(create_url, headers=headers, json=payload)
if r.status_code not in (200, 201):
    print("Failed to create PR:", r.status_code, r.text)
    sys.exit(1)

pr = r.json()
print("Created PR:", pr.get("html_url"))
