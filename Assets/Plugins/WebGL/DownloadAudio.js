// DownloadAudio.js
function downloadAudio(base64Data, fileName) {
    var link = document.createElement('a');
    link.href = 'data:audio/wav;base64,' + base64Data;
    link.download = fileName;
    link.click();
}
