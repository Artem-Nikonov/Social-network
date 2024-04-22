document.addEventListener("DOMContentLoaded", DOMContentLoaded);
//document.addEventListener("DOMContentLoaded", getChats);

let userIdInput;
let addUserBtn;

let chatId
let userGroup

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`/chat`)
    .build(); 

function DOMContentLoaded() {
    userIdInput = document.getElementById("userId");
    addUserBtn = document.getElementById("addUserBtn");

    addUserBtn.addEventListener("click", addUserBtnClick);
    chatId = document.getElementById("chatId").getAttribute("data");
    userGroup =`ch${chatId}`;
    document.getElementById("sendBtn").addEventListener("click", sendMessage)
}


function sendMessage () {
    const message = document.getElementById("message").value;
    console.log("hi")
    hubConnection.invoke("Send", message, userGroup) // отправка данных серверу
        .catch(function (err) {
            return console.error(err.toString());
        });
}

// получение данных с сервера
hubConnection.on("Receive", function (message, userName) {
    const userNameElem = document.createElement("b");
    userNameElem.textContent = `${userName}: `;
    const elem = document.createElement("p");
    elem.appendChild(userNameElem);
    elem.appendChild(document.createTextNode(message));

    document.getElementById("chatroom").appendChild(elem);
});

hubConnection.start()
    .then(function () {
        document.getElementById("sendBtn").disabled = false;
        hubConnection.invoke("Enter", userGroup);
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

async function addUserBtnClick() {
    const userId = parseInt(userIdInput.value);
    if (!userId) {
        showError("Id пользователя должен быть числом");
        //return;
    }
    try {
        const response = await fetch(`/myChats/${chatId}?act=addUser&userId=${userId}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
        });

        if (response.redirected) {
            console.log(response.url);
            return;
        }
        if (response.ok) {
            console.log("добавлен");
            userIdInput.value = "";
        }
        else {
            const error = `${response.status}. Произошла ошибка при выполнении запроса`
            showError(error);
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
};