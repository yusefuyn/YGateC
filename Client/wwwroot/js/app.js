window.custom_confirm = function (message, title = 'Yussefuynstein',yesbtn='Evet',nobtn='Hayır') {
    return new Promise((resolve, reject) => {
        var modal = document.getElementById('myModal');
        var modalHelpText = document.getElementById('modalDescription');
        var modalTitle = document.getElementById('modalTitle');

        modalHelpText.textContent = message;
        modalTitle.textContent = title;

        var bootstrapModal = new bootstrap.Modal(modal);
        bootstrapModal.show();

        var okButton = modal.querySelector('.ok');
        okButton.textContent = yesbtn;
        okButton.onclick = function () {
            // Modal'ı kapat
            bootstrapModal.hide();

            // Promise'yi resolve ederek true döndür
            resolve(true);
        };
        var cancelButton = modal.querySelector('.cancel');
        cancelButton.textContent = nobtn;
        cancelButton.onclick = function () {
            bootstrapModal.hide();
            // Promise'yi resolve ederek false döndür
            resolve(false);
        };
    });
};

let notificationCount = 0; // Bildirimlerin sayısını takip et

function showNotification(message) {
    const notification = document.createElement('div');
    notification.classList.add('notification');
    notification.innerHTML = `
    <button id="notification-close-btn">X</button>
    <p id="notification-message">${message}</p>
  `;

    notification.style.top = `${20 + notificationCount * 80}px`; // Her yeni bildirim 80px yukarıda olacak

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 10); // Kısa bir gecikme ile görsel efekt ekleyelim

    setTimeout(() => {
        hideNotification(notification);
    }, 5000); // 5 saniye sonra kaybolacak

    notification.querySelector('#notification-close-btn').addEventListener('click', () => {
        hideNotification(notification);
    });

    notificationCount++;
}


function ycustomimagelistComponentLoad() {
    const customListComponent = document.querySelector('ycustomimagelist customlistcomponent');

    if (!customListComponent) {
        return;
    }

    const listComponents = customListComponent.parentElement.querySelectorAll('listcomponent');

    if (listComponents.length === 0) {
        return;
    }

    customListComponent.innerHTML = '';

    listComponents.forEach(listComponent => {
        const imageUrl = listComponent.textContent.trim();

        if (!imageUrl) {
            return; // Boş olduğunda, bu iteration'ı atlıyoruz
        }

        const img = document.createElement('img');
        img.src = imageUrl;
        img.alt = "Image";
        img.style.maxWidth = "800px";
        img.style.maxHeight = "600px";
        img.style.width = "auto";
        img.style.height = "auto";

        customListComponent.appendChild(img);
    });
}



function hideNotification(notification) {
    notification.classList.remove('show');
    notification.classList.add('hide');

    setTimeout(() => {
        notification.remove();
        notificationCount--; // Bildirim sayısını azalt
    }, 500); // Gizlendikten sonra öğeyi DOM'dan sil
}

window.applyCss = function (css) {
    var style = document.createElement('style');
    style.innerHTML = css;
    document.head.appendChild(style);
};


window.initializeSummernote = function (htmlName) {
    $(document).ready(function () {
        $("#" + htmlName).summernote({
            height: 300,
            minHeight: 400,
            maxHeight: null,
        });
    });
};

window.getSummernoteContent = function (htmlName) {
    return $("#" + htmlName).summernote('code');
};

window.changeSummernoteFontColor = function (htmlName, color) {
    $("#" + htmlName).summernote('foreColor', color);
};

//window.registerEditorChangeEvent = (elementId,veriable) => {
//    $('#' + elementId).on('summernote.change', function (e, contents) {
//        veriable = contents;
//    });
//};