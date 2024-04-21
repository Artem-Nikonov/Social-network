﻿document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getUsers);
let userId = document.getElementById("PageId").getAttribute("data");
let pageId = 1;//id пагинации
let usersContainer;
let addLoadUsersBtn;

function DOMContentLoaded() {
    usersContainer = document.getElementById("pagesContainer");
    addLoadUsersBtn = document.getElementById("AddLoadDataBtn");
    addLoadUsersBtn.addEventListener("click", getUsers);
}

async function getUsers() {
    try {
        const response = await fetch(`/users/${userId}/subscribers/list?page=${pageId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Пользователи получены");
            usersData = await response.json()
            console.log(usersData)
            for (let user of usersData.users) {
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