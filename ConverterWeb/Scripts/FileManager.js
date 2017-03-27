(function () {

    var btnUpload = document.getElementById('btnUpload');
    var fileCtrl = document.getElementById('fileCtrl');
    var lnkFile = document.getElementById('lnkFile');
    var successDiv = document.getElementById('success');
    var errorDiv = document.getElementById('error');

    btnUpload.addEventListener('click', function () {

        var formData;
        var promise;

        // Reset results output
        errorDiv.innerHTML = '';
        errorDiv.style.display = 'none';
        successDiv.style.display = 'none';

        // Ensure user has chosen a file
        if (fileCtrl.value) {

            // Append file to form data
            formData = new FormData();
            formData.append('sourceFile', fileCtrl.files[0], fileCtrl.files[0].name);

            // Display modal screen
            vex.dialog.alert({ content: 'Processing file. Please wait', showCloseButton: false, buttons:[] });

            // Post file for conversion
            promise = DATA.post('api/v1/files', formData);

            promise.then(function (resourceUrl) {

                // Close modal screen
                vex.closeAll();

                successDiv.style.display = 'block';
                lnkFile.setAttribute('href', resourceUrl);
                lnkFile.innerHTML = resourceUrl;
            });

            promise.catch(function (reason) {
                errorDiv.style.display = 'block';
                errorDiv.innerHTML = reason;
            });
        }
        else {
            vex.dialog.alert('Please select a file');
        }
    });

}());

var DATA = (function () {

    function post(url, formData) {

        return new Promise(function (resolve, reject) {

            var req = new XMLHttpRequest();
            req.open('POST', url);

            req.onload = function () {

                if (req.status === 201) {
                    resolve(req.getResponseHeader('Location'));
                }
                else {
                    reject(Error(req.statusText));
                }
            };

            req.onerror = function () {
                reject(Error("Network Error"));
            };

            if (formData) {
                req.send(formData);
            }
            else {
                req.send();
            }

        });
    }

    return {
        post: post
    };

}());