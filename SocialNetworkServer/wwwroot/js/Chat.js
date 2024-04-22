document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getUsers);

let userIdInput;
let addUserBtn;
let chatId;
let userGroup;
let pageId = 1;//id пагинации
let usersContainer;
let addLoadUsersBtn;

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`/chat`)
    .build(); 

function DOMContentLoaded() {
    userIdInput = document.getElementById("userId");
    addUserBtn = document.getElementById("addUserBtn");
    usersContainer = document.getElementById("usersContainer");
    addLoadUsersBtn = document.getElementById("addLoadUsersBtn");

    addUserBtn.addEventListener("click", addUserBtnClick);
    addLoadUsersBtn.addEventListener("click", getUsers);
    chatId = parseInt(document.getElementById("chatId").getAttribute("data"));
    document.getElementById("sendBtn").addEventListener("click", sendMessage);
    userGroup = `ch${chatId}`;
}


// получение данных с сервера
hubConnection.on("Receive", ReceiveMessage);

hubConnection.start()
.then(function () {
    document.getElementById("sendBtn").disabled = false;
    hubConnection.invoke("Enter", chatId);
})
.catch(function (err) {
    return console.error(err.toString());
});


function sendMessage() {
    const message = document.getElementById("message").value;
    console.log("hi")
    hubConnection.invoke("Send", message, chatId) // отправка данных серверу
        .catch(function (err) {
            return console.error(err.toString());
        });
}
function ReceiveMessage(messageInfo, userInfo) {
    const userNameElem = createUserLink(userInfo);
    const meta = document.createElement("p");
    meta.textContent = messageInfo.sendingDate;
    const elem = document.createElement("p");
    elem.appendChild(userNameElem);
    elem.appendChild(meta);
    elem.appendChild(document.createTextNode(`: ${messageInfo.content}`));

    document.getElementById("chatroom").appendChild(elem);
}

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

async function getUsers() {
    try {
        const response = await fetch(`/myChats/${chatId}/users?page=${pageId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Пользователи получены");
            usersData = await response.json()
            console.log(usersData)
            for (let user of usersData.items) {
                usersContainer.appendChild(createUserDiv(user));
            }
            lastPageHandler(usersData.meta);
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function createUserDiv(userInfo) {
    let userDiv = document.createElement("div");
    userDiv.appendChild(createUserLink(userInfo));
    return userDiv;
}

function createUserLink(userInfo) {
    let userLink = document.createElement("a");
    userLink.classList.add("custom_link");
    userLink.setAttribute("href", `/users/${userInfo.userId}`);
    userLink.textContent = `${userInfo.userName} ${userInfo.userSurname}`;
    return userLink;
}

function lastPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadUsersBtn.style.display = "none";
    }
    else {
        addLoadUsersBtn.style.display = "inline-block";
    }
    pageId = pageMetaData.pageId + 1;
}