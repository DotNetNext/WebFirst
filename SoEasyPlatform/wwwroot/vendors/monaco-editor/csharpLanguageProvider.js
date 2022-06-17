function registerCsharpProvider() {

    var assemblies = null;

    monaco.languages.registerCompletionItemProvider('csharp', {
        triggerCharacters: [".", "("],
        provideCompletionItems: async (model, position) => {
            let suggestions = [];

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/complete", JSON.stringify(request))

            for (let elem of resultQ.data) {
                suggestions.push({
                    label: elem.Suggestion,
                    kind: monaco.languages.CompletionItemKind.Function,
                    insertText: elem.Suggestion,
                    documentation: elem.Description
                });
            }

            return { suggestions: suggestions };
        }
    });

    monaco.languages.registerHoverProvider('csharp', {
        provideHover: async function (model, position) {

            let request = {
                Code: model.getValue(),
                Position: model.getOffsetAt(position),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/hover", JSON.stringify(request))

            posStart = model.getPositionAt(resultQ.data.OffsetFrom);
            posEnd = model.getPositionAt(resultQ.data.OffsetTo);

            return {
                range: new monaco.Range(posStart.lineNumber, posStart.column, posEnd.lineNumber, posEnd.column),
                contents: [
                    { value: resultQ.data.Information }
                ]
            };
        }
    });

    monaco.editor.onDidCreateModel(function (model) {
        async function validate() {

            let request = {
                Code: model.getValue(),
                Assemblies: assemblies
            }

            let resultQ = await axios.post("/completion/codeCheck", JSON.stringify(request))

            let markers = [];

            for (let elem of resultQ.data) {
                posStart = model.getPositionAt(elem.OffsetFrom);
                posEnd = model.getPositionAt(elem.OffsetTo);

                markers.push({
                    severity: elem.Severity,
                    startLineNumber: posStart.lineNumber,
                    startColumn: posStart.column,
                    endLineNumber: posEnd.lineNumber,
                    endColumn: posEnd.column,
                    message: elem.Message
                });
            }

            monaco.editor.setModelMarkers(model, 'csharp', markers);
        }

        var handle = null;
        model.onDidChangeContent(() => {
            monaco.editor.setModelMarkers(model, 'csharp', []);
            clearTimeout(handle);
            handle = setTimeout(() => validate(), 500);
        });
        validate();
    });

}