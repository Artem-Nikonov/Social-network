document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getMessages);
document.addEventListener("DOMContentLoaded", getUsers);

let userIdInput;
let addUserBtn;
let addLoadUsersBtn;
let addLoadMessagesBtn;

let chatId;
let userGroup;

let usersPageId = 1;//id пагинации
let startMessageId;

let messageTextInput;
let mainMessagesContainer;
let messagesContainer;
let usersContainer;


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(`/chat`)
    .build(); 

function DOMContentLoaded() {
    mainMessagesContainer = document.getElementById("mainMessagesContainer");
    messagesContainer = document.getElementById("messagesContainer");
    addLoadMessagesBtn = document.getElementById("addLoadMessagesBtn");
    userIdInput = document.getElementById("userId");
    addUserBtn = document.getElementById("addUserBtn");
    messageTextInput = document.getElementById("messageInput");
    usersContainer = document.getElementById("usersContainer");
    addLoadUsersBtn = document.getElementById("addLoadUsersBtn");

    addLoadMessagesBtn.addEventListener("click", getMessages);
    addUserBtn.addEventListener("click", addUserBtnClick);
    addLoadUsersBtn.addEventListener("click", getUsers);
    chatId = parseInt(document.getElementById("chatId").getAttribute("data"));
    document.getElementById("sendBtn").addEventListener("click", sendMessage);
    messageTextInput.addEventListener("keydown", function (event) {
        if (event.ctrlKey && event.key === "Enter")
            sendMessage();
    });
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
    const message = messageTextInput.value;
    if (message.length == 0) return;
    hubConnection.invoke("Send", message, chatId) // отправка данных серверу
        .catch(function (err) {
            return console.error(err.toString());
        });
    messageTextInput.value = "";
}
function ReceiveMessage(messageInfo) {
    messagesContainer.appendChild(createMessageDiv(messageInfo))
}


async function getMessages() {
    try {
        const response = await fetch(`/myChats/${chatId}/messages?startMessageId=${startMessageId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("сообщения получены");
            messagesData = await response.json()
            console.log(messagesData)

            const containerHeightBeforeLoad = messagesContainer.scrollHeight;
            const scrollTopBeforeLoad = mainMessagesContainer.scrollTop;

            let lastMsgElrment = messagesContainer.firstElementChild;

            for (let message of messagesData.items) {
                let messageDiv = createMessageDiv(message);
                messagesContainer.insertBefore(messageDiv, lastMsgElrment);
                lastMsgElrment = messageDiv;
            }
            lastMessagesPageHandler(messagesData.meta)
            const containerHeightAfterLoad = messagesContainer.scrollHeight;
            const addedHeight = containerHeightAfterLoad - containerHeightBeforeLoad;

            // Установить скроллбар обратно на позицию, соответствующую сохраненной позиции скролла и добавленной высоте
            mainMessagesContainer.scrollTop = scrollTopBeforeLoad + addedHeight;

        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
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
//=================================
async function getUsers() {
    try {
        const response = await fetch(`/myChats/${chatId}/users?page=${usersPageId}`, {
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
            lastUsersPageHandler(usersData.meta);
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}




function createMessageDiv(messageInfo) {
    let messageDiv = document.createElement("div");
    messageDiv.classList.add("message");
    messageDiv.appendChild(createMetaDataDiv(messageInfo));
    messageDiv.appendChild(createMessageContentDiv(messageInfo));
    return messageDiv;
}

function createMessageContentDiv(messageInfo) {
    let contentDiv = document.createElement("div");
    contentDiv.classList.add("message_content");
    let content = document.createElement("pre");
    content.classList.add("message_content_pre");
    content.textContent = messageInfo.content;
    contentDiv.appendChild(content)
    return contentDiv;
}

function createTimeSpanDiv(time) {
    let timeSpanDiv = document.createElement("div");
    timeSpanDiv.classList.add("post_data_section");
    let timeSpan = document.createElement("span");
    timeSpan.textContent = time;
    timeSpanDiv.appendChild(timeSpan);
    return timeSpanDiv;
}

function createMetaDataDiv(messageInfo) {
    let metaDataDiv = document.createElement("div");
    metaDataDiv.classList.add("meta_data_section")
    metaDataDiv.appendChild(createUserLink(messageInfo.userInfo));
    metaDataDiv.appendChild(createTimeSpan(messageInfo.sendingDate));
    return metaDataDiv;
}

function createTimeSpan(time) {
    let timeSpan = document.createElement("span");
    timeSpan.classList.add("time_span");
    timeSpan.textContent = time;
    return timeSpan;
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


function lastUsersPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadUsersBtn.style.display = "none";
    }
    else {
        addLoadUsersBtn.style.display = "inline-block";
    }
    usersPageId = pageMetaData.pageId + 1;
}

function lastMessagesPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadMessagesBtn.style.display = "none";
    }
    else {
        addLoadMessagesBtn.style.display = "inline-block";
    }
    startMessageId = pageMetaData.lastItemId - 1;
}