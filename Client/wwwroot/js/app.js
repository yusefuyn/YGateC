function enableButton(buttonId) {
    var button = document.getElementById(buttonId);
    if (button) {
        button.disabled = false;
    } else {
        console.log('Button not found');
    }
}

function disableButton(buttonId) {
    var button = document.getElementById(buttonId);
    if (button) {
        button.disabled = true;
    } else {
        console.log('Button not found');
    }
}



window.custom_confirm = function (message, title = 'Yussefuynstein', yesbtn = 'Evet', nobtn = 'Hayır') {
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

    notification.style.top = `${20 + notificationCount * 80}px`;

    document.body.appendChild(notification);

    setTimeout(() => {
        notification.classList.add('show');
    }, 10);

    setTimeout(() => {
        hideNotification(notification);
    }, 5000);

    notification.querySelector('#notification-close-btn').addEventListener('click', () => {
        hideNotification(notification);
    });

    notificationCount++;
}
function buyukResimGuncelle(resimUrl) {
    var buyukResim = document.getElementById('buyukResim');
    buyukResim.style.backgroundImage = 'url(' + resimUrl + ')';
}

function ycustomimagelistComponentLoad() {
    const customListComponent = document.querySelector('ycustomimagelist customlistcomponent');
    let currentResimIndex = 0;

    if (!customListComponent) {
        return;
    }

    const listComponents = customListComponent.parentElement.querySelectorAll('listcomponent');

    if (listComponents.length === 0) {
        return;
    }

    customListComponent.innerHTML = '';
    customListComponent.classList.add("container");
    customListComponent.style.borderRadius = '7px';
    customListComponent.style.margin = '5px';


    const buyukResimDiv = document.createElement('div');
    buyukResimDiv.classList.add('buyuk-resim');
    buyukResimDiv.onclick = function () {
        // TODO : Resim hızlı cercevesiz bir popup ile açılsın kenarlara tıklanınca cıkılsın.
    };
    buyukResimDiv.id = 'buyukResim';

    const buyukResimBaslik = document.createElement('p');
    buyukResimBaslik.id = 'buyukResimBaslik';
    buyukResimBaslik.textContent = 'Büyük Resim Başlığı';
    buyukResimBaslik.style.color = "#00000000";

    const solTus = document.createElement('button');
    solTus.id = 'solTus';
    solTus.classList.add('navigasyon-tusu');
    solTus.innerHTML = '&lt;'; 

    const sagTus = document.createElement('button');
    sagTus.id = 'sagTus';
    sagTus.classList.add('navigasyon-tusu');
    sagTus.innerHTML = '&gt;';

    solTus.onclick = function () {
        if (currentResimIndex > 0) {
            currentResimIndex--;
        } else {
            currentResimIndex = resimUrls.length - 1; 
        }
        buyukResimGuncelle(resimUrls[currentResimIndex]);
    };

    sagTus.onclick = function () {
        if (currentResimIndex < resimUrls.length - 1) {
            currentResimIndex++;
        } else {
            currentResimIndex = 0; 
        }
        buyukResimGuncelle(resimUrls[currentResimIndex]);

    };

    buyukResimDiv.appendChild(solTus);
    buyukResimDiv.appendChild(buyukResimBaslik);
    buyukResimDiv.appendChild(sagTus);

    document.body.appendChild(buyukResimDiv);

    customListComponent.appendChild(buyukResimDiv);

    const resimContainer = document.createElement('div');
    resimContainer.classList.add('resim-container');

    const resimUrls = [];

    listComponents.forEach(listComponent => {
        const imageUrl = listComponent.textContent.trim();

        if (!imageUrl) {
            return; // Boş olduğunda, bu iteration'ı atlıyoruz
        }

        const img = document.createElement('img');
        img.src = imageUrl;
        img.alt = "Image";
        img.onclick = function () {
            buyukResimGuncelle(imageUrl);
        };

        resimUrls.push(imageUrl);
        resimContainer.appendChild(img);
    });

    customListComponent.appendChild(resimContainer);
}



function hideNotification(notification) {
    notification.classList.remove('show');
    notification.classList.add('hide');

    setTimeout(() => {
        notification.remove();
        notificationCount--;
    }, 500); 
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
            minHeight: 160000,
            maxHeight: null,
            callbacks: {
                onChange: function (contents, $editable) {
                    console.log("Source updated");
                }
            }
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