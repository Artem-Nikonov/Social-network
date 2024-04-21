document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getChats);

let chatNameInput;
let createChatBtn;
let chatsContainer;
let addLoadChatsBtn;
let pageId = 1;//id пагинации

function DOMContentLoaded() {
    chatNameInput = document.getElementById("chatName");
    chatsContainer = document.getElementById("chatsContainer");
    createChatBtn = document.getElementById("createChatBtn");
    addLoadChatsBtn = document.getElementById("AddLoadChatsBtn");
    createChatBtn.addEventListener("click", createChatBtnClick);
    addLoadChatsBtn.addEventListener("click", getChats);
}

async function createChatBtnClick() {
    const chatName = chatNameInput.value;
    if (chatName.length < 1) {
        showError("Длина названия чата должна быть не менее 1-го символа.");
        return;
    }
    const chatData = {
        ChatName: chatName
    };
    console.log(chatData)
    try {
        const response = await fetch("/myChats/create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(chatData)
        });

        if (response.redirected) {
            console.log(response.url);
            return;
        }
        if (response.ok) {
            console.log("чат создан");
            chatName.value = "";
            const chatInfo = await response.json();
            const chat = createChatDiv(chatInfo);
            let lastChatInCintainer = chatsContainer.firstChild;
            chatsContainer.insertBefore(chat, lastChatInCintainer)
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

async function getChats() {
    try {
        const response = await fetch(`/myChats/list?page=${pageId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Чаты получены");
            chatsData = await response.json()
            console.log(chatsData)
            for (let chat of chatsData.items) {
                chatsContainer.appendChild(createChatDiv(chat));
            }
            lastPageHandler(chatsData.meta);
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function createChatDiv(chatInfo) {
    let chatDiv = document.createElement("div");
    chatDiv.appendChild(createChatLink(chatInfo));
    return chatDiv;
}

function createChatLink(chatInfo) {
    let chatLink = document.createElement("a");
    chatLink.classList.add("custom_link");
    chatLink.setAttribute("href", `/myChats/${chatInfo.chatId}`);
    chatLink.textContent = `${chatInfo.chatName}`;
    return chatLink;
}

function lastPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadChatsBtn.style.display = "none";
    }
    else {
        addLoadChatsBtn.style.display = "inline-block";
    }
    pageId = pageMetaData.pageId + 1;
}