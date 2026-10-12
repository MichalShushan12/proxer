import * as vscode from 'vscode';

export function activate(context: vscode.ExtensionContext) {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';// למחוק את זה שהפרויקט נמצא בענן כי אין צורך בזה

    let disposable = vscode.commands.registerCommand('proxer.refactor', async () => {
        const editor = vscode.window.activeTextEditor;
        if (!editor) {
            vscode.window.showWarningMessage('No active editor');
            return;
        }

        let selectedCode = editor.document.getText(editor.selection);
        if (!selectedCode) {
            selectedCode = editor.document.getText();
        }

        if (!selectedCode) {
            vscode.window.showWarningMessage('The file is empty');
            return;
        }

        const options = [
            { label: '⚡Shorten & improve code', value: 'ShortenCode' },
            { label: '📝 Add comments & docs', value: 'AddComments' },
            { label: '🐛 Fix bugs & errors', value: 'FixBugs' },
            { label: '💡 Explain the code', value: 'ExplainCode' }
        ];

        const selectedOption = await vscode.window.showQuickPick(options, {
            placeHolder: 'What should Proxer do?'
        });
        if (!selectedOption) { return; }

        // שליפת שפת הממשק באופן דינמי (למשל "he" או "he-IL")
        // ה-Backend ב-C# משתמש ב-CultureInfo.GetCultureInfo כדי להמיר זאת ל-"Hebrew"
        const configured = vscode.workspace.getConfiguration('proxer').get<string>('language');
        const userLanguage = configured || vscode.env.language || 'en';

        vscode.window.withProgress({
            location: vscode.ProgressLocation.Notification,
            title: `Proxer AI: Processing...`,
            cancellable: false
        }, async () => {
            try {
                const serverUrl = vscode.workspace.getConfiguration('proxer').get<string>('serverUrl')
                    || 'https://localhost:7201';
                const response = await fetch(`${serverUrl}/api/RefactorLog/process`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        originalCode: selectedCode,
                        actionType: selectedOption.value,
                        targetLanguage: userLanguage
                    })
                });

                if (!response.ok) {
                    const err = await response.text();
                    vscode.window.showErrorMessage(`Server error: ${err}`);
                    return;
                }

                const data: any = await response.json();
                const resultText = data.correctedCode || data.CorrectedCode;

                if (!resultText) {
                    vscode.window.showWarningMessage('The server returned no response.');
                    return;
                }

                // אם נבחר הסבר, מציגים במסמך Markdown
                if (selectedOption.value === 'ExplainCode') {
                    const doc = await vscode.workspace.openTextDocument({
                        content: resultText,
                        language: 'markdown'
                    });
                    await vscode.window.showTextDocument(doc, { preview: false });
                }
                // אם נבחר שינוי קוד, מציגים בחלון Diff
                else {
                    const originalDoc = await vscode.workspace.openTextDocument({
                        content: selectedCode,
                        language: editor.document.languageId
                    });
                    const newDoc = await vscode.workspace.openTextDocument({
                        content: resultText,
                        language: editor.document.languageId
                    });

                    await vscode.commands.executeCommand(
                        'vscode.diff',
                        originalDoc.uri,
                        newDoc.uri,
                        'Proxer AI: Original code ↔ Suggested code'
                    );
                }
            } catch (error: any) {
                vscode.window.showErrorMessage(`Error communicating with the API: ${error.message}`);
            }
        });
    });

    context.subscriptions.push(disposable);
}

export function deactivate() { }