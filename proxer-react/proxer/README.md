# Proxer 

AI-powered code assistant for VS Code. Refactor your code and get explanations
**in your own language**.

[▶ Watch the demo](/docs/demo.gif.gif)


## Architecture

VS Code Extension (TypeScript) → ASP.NET Core API → Gemini API
                                        ↓
                                   SQL Server (request logs)

- **Extension:** collects the selection, sends it to the API, shows the result as Markdown or a diff view.
- **API:** layered design (Controller → Service → Repository) with Entity Framework.
- **Language detection:** the extension sends VS Code's UI language (or `proxer.language`); the server maps it to a language name with `CultureInfo` and instructs the model accordingly.

## Features

- ⚡ Shorten & improve code
- 📝 Add comments & documentation
- 🐛 Fix bugs & errors
- 💡 Explain the code, in your language (English, Hebrew, German, Spanish, Arabic, and more)

## Getting Started

1. Clone the repo.
2. Get a Gemini API key from Google AI Studio and store it with user secrets (run inside `Proxer/`):
   `dotnet user-secrets set "Gemini:ApiKey" "<your-key>"`
3. Run the SQL script in the repo root (`SQLQuery1.sql`) to create the database, set the connection string in `Proxer/appsettings.json`, and run the API.
4. In `proxer-react/proxer/`: `npm install`, `npm run compile`, then press F5.


> Note: the free Gemini tier has a daily request limit.


## Usage

1. Select code in the editor (or leave nothing selected to use the whole file).
2. Right-click and choose **Proxer: Refactor Code with AI**
   (or open the Command Palette and run the same command).
3. Pick an action.
4. Explanations open as a Markdown document. Code changes open in a diff view
   so you can review them before applying.

## Extension Settings

| Setting | Description |
|---|---|
| `proxer.language` | Language for explanations and comments (e.g. `he`, `de`, `ar`). If empty, VS Code's display language is used. |
| `proxer.serverUrl` | Address of the Proxer server (default: `https://localhost:7201`, i.e. the server runs locally). |

## Technologies

TypeScript, VS Code Extension API, C#/.NET, ASP.NET Core, Entity Framework, SQL Server, Gemini API, REST.

## Privacy

The selected code is sent to the Proxer server and processed by Google Gemini.
Requests are also logged in the server database. Do not use it on code you are not allowed to share.

## License

MIT



