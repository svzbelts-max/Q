# Q
проба

# Auto PR Bot (example)

Этот пример добавляет в репозиторий рабочий GitHub Actions workflow и Python-скрипт, которые:
- читают задачу из `BOT_INSTRUCTIONS.md`,
- запрашивают у OpenAI патч в формате unified diff,
- применяют патч, создают ветку, пушат и открывают Pull Request.

Важно: это шаблон и экспериментальный поток. Перед включением в продакшен прочитайте раздел "Безопасность".

## Как включить (шаги)
1. В репозиторий Q добавьте файлы:
   - `.github/workflows/auto-update.yml`
   - `scripts/auto_pr.py`
   - `scripts/requirements.txt`
   - создайте `BOT_INSTRUCTIONS.md` в корне с описанием задачи для бота.

2. В репозитории в Settings → Secrets and variables → Actions добавьте секреты:
   - `BOT_PAT` — Personal Access Token (или использование GitHub App токена). PAT должен иметь scope `repo` (минимум: repo:contents write, pull_request write).
   - `OPENAI_API_KEY` — ключ OpenAI для создания патчей (если будете использовать другой LLM, адаптируйте скрипт).

3. В `BOT_INSTRUCTIONS.md` опишите чётко задачу, пример:
   ```
   Обновить зависимость `requests` в requirements.txt до последней стабильной версии и исправить код, если есть несовместимости.
   ```

4. Запустите workflow вручную в Actions → Auto PR bot → Run workflow или дождитесь по cron.

## Рекомендации по безопасности
- Лучше использовать GitHub App вместо PAT: App даёт гибкие права и легче отслеживается.
- Ограничьте репозиторий/организацию, где бот установлен.
- Включите проверки CI (tests, linters) и не вливайте автоматически в `main` — используйте PR для ревью.
- Логи и содержимое ответов LLM могут раскрывать код — храните ключи в секрете.

## Что делать если что-то пошло не так
- Закройте PR, откатите ветку: `git push origin --delete <branch>` и `git branch -D <branch>`.
- Отключите workflow, удалив `auto-update.yml`.
