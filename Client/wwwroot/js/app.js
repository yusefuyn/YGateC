
async function callSaveParameterFromJS(parameters) {
    const result = await DotNet.invokeMethodAsync('YGate.Client', 'SaveParameter', parameters);
    console.log(result);  // Sonuç ile ilgili işlem yapabilirsiniz.
}

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

function ycustomPictureListComponentLoad() {
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

function ycustomComboListComponentLoad() {
    const customListComponent = document.querySelector('customlistcomponent');
    let currentResimIndex = 0;

    if (!customListComponent) {
        return;
    }

    const listComponents = customListComponent.parentElement.querySelectorAll('combocomponent');

    if (listComponents.length === 0) {
        return;
    }

    customListComponent.classList.add("container");
    customListComponent.style.borderRadius = '7px';
    customListComponent.style.margin = '5px';

    // İlk olarak tüm mevcut içeriği temizliyoruz, böylece eski içerik kalmaz
    customListComponent.innerHTML = '';

    // Her bir listComponent için bir checkbox ekliyoruz
    listComponents.forEach(listComponent => {
        const deger = listComponent.textContent.trim();

        if (!deger) {
            return; // Eğer değer boşsa, bu iteration'ı atlıyoruz
        }

        // Checkbox öğesi oluşturuluyor
        const checkboxContainer = document.createElement('div');
        checkboxContainer.classList.add('form-check');

        // Checkbox elemanını oluşturuyoruz
        const checkboxElement = document.createElement('input');
        checkboxElement.type = 'checkbox';
        checkboxElement.classList.add('form-check-input');
        checkboxElement.checked = true;
        checkboxElement.disabled = true;
        // Checkbox'ın label'ını oluşturuyoruz
        const labelElement = document.createElement('label');
        labelElement.textContent = deger;
        labelElement.classList.add('form-check-label');
        // Checkbox ve label'ı container'a ekliyoruz
        checkboxContainer.appendChild(checkboxElement);
        checkboxContainer.appendChild(labelElement);

        // checkboxContainer'ı customListComponent'e ekliyoruz
        customListComponent.appendChild(checkboxContainer);
    });
}

function ycustomlistComponentLoad() {
    ycustomPictureListComponentLoad();
    ycustomComboListComponentLoad();
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


window.initializeSummernote = function (htmlName, dotnetHelper) {
    $(document).ready(function () {
        $("#" + htmlName).summernote({
            height: 400,
            minHeight: 400,
            maxHeight: null,
            callbacks: {
                onChange: function (contents, $editable) {
                    // DotNetObjectReference üzerinden C# metodunu çağırıyoruz
                    dotnetHelper.invokeMethodAsync('UpdateContent', contents)
                        .then(result => {
                            console.log('C# metodu başarılı bir şekilde çağrıldı.');
                        })
                        .catch(error => {
                            console.error('C# metoduna çağrı hatası:', error);
                        });
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

function setThemeJSCode() {
    const mainApp = document.querySelector('.main-app');
    const icon = document.getElementById('themeButton-Icon');
    // background-image'i değiştirme

    // Tema renklerini değiştir
    const root = document.documentElement;
    let backgroundColor = getComputedStyle(document.documentElement).getPropertyValue('--background-color');

    if (backgroundColor === '#040512') {
        // Açık tema
        root.style.setProperty('--background-color', '#FBF9ED'); // Zıt renk
        root.style.setProperty('--main-background-color', '#F6F4E6'); // Zıt renk
        root.style.setProperty('--font-color', '#1F1F1F'); // Zıt renk
        root.style.setProperty('--panel-background', '#E1E1E1'); // Zıt renk
        root.style.setProperty('--panel-border', '#CCC'); // Zıt renk
        root.style.setProperty('--button-color', '#11111123'); // Zıt renk
        root.style.setProperty('--button-border-color', '#E5E2DE'); // Zıt renk
        root.style.setProperty('--button-hover-color', '#649A49'); // Zıt renk
        root.style.setProperty('--circle-color', '#649A4965'); // Zıt renk
        root.style.setProperty('--panel-complete', '#C474C4'); // Zıt renk
        root.style.setProperty('--panel-inprogress', '#F3290F'); // Zıt renk
        root.style.setProperty('--panel-planned', '#C5C5BF'); // Zıt renk
        root.style.setProperty('--box-shadow', '3px 3px 10px rgba(255, 255, 255, 0.4), -1px -1px 6px rgba(0, 0, 0, 1)'); // Zıt renk
        icon.classList.remove(icon.classList[1]);
        icon.classList.add('fa-moon');
        mainApp.style.backgroundImage = "url('/glow-page-light.png')";

    } else {
        // Koyu tema (orijinal tema)
        root.style.setProperty('--background-color', '#040512');
        root.style.setProperty('--main-background-color', '#090B19');
        root.style.setProperty('--font-color', '#e0e0e0');
        root.style.setProperty('--panel-background', '#1e1e1e');
        root.style.setProperty('--panel-border', '#333');
        root.style.setProperty('--button-color', '#ffffff06');
        root.style.setProperty('--button-border-color', '#1a1d21');
        root.style.setProperty('--button-hover-color', '#649A49');
        root.style.setProperty('--circle-color', '#649A4965');
        root.style.setProperty('--panel-complete', '#3b8b3b');
        root.style.setProperty('--panel-inprogress', '#0dcaf0');
        root.style.setProperty('--panel-planned', '#343a40');
        root.style.setProperty('--box-shadow', '3px 3px 10px rgba(0, 0, 0, 1), -1px -1px 6px rgba(255, 255, 255, 0.4)');
        icon.classList.remove(icon.classList[1]);
        icon.classList.add('fa-sun');
        mainApp.style.backgroundImage = "url('/glow-page-dark.png')";

    }
}

