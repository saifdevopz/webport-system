function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/pdf;base64," + encodeURIComponent(bytesBase64);
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

window.downloadFileFromStream = async (fileName, streamRef) => {
    const blob = await streamRef.arrayBuffer();
    const url = URL.createObjectURL(new Blob([blob], { type: 'application/pdf' }));
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    URL.revokeObjectURL(url);
}