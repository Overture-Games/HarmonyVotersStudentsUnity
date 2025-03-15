mergeInto(LibraryManager.library, {
    UNITY_SAVE: function(content, name, mimetype) {
        var blob = new Blob([Pointer_stringify(content)], { type: Pointer_stringify(mimetype) });
        var link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = Pointer_stringify(name);
        link.click();
    }
});