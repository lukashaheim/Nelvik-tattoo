window.addEventListener("load", function () {
    var qrCodeData = document.getElementById("qrCodeData");
    var qrCodeElement = document.getElementById("qrCode");

    if (!qrCodeData || !qrCodeElement) return;

    var uri = qrCodeData.getAttribute("data-url");
    if (!uri) return;

    // QRCode comes from qrcode.min.js
    new QRCode(qrCodeElement, {
        text: uri,
        width: 200,
        height: 200
    });
});
