document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", function () {
    getUsers(null);
});

let pageId = 1;
let usersContainer;
let addLoadUsersBtn;

let searchField;
let searchBtn;

function DOMContentLoaded() {
    usersContainer = document.getElementById("usersContainer");
    addLoadUsersBtn = document.getElementById("AddLoadUsersBtn");
    searchField = document.getElementById("searchField");
    searchBtn = document.getElementById("searchBtn");
    addLoadUsersBtn.addEventListener("click", function () {
        var searchQuery = searchField.value;
        getUsers(searchQuery);
    });

    searchBtn.addEventListener("click", function () {
        var searchQuery = searchField.value;
        usersContainer.innerHTML = '';
        pageId = 1;
        getUsers(searchQuery);
    });
}


async function getUsers(filter) {
    try {
        let path = filter ? `/users/list?page=${pageId}&filter=${filter}` : `/users/list?page=${pageId}`;
        const response = await fetch(path, {
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
            showError("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function createUserDiv(userInfo) {
    let userDiv = document.createElement("div");
    let userLinkSection = document.createElement("div");
    let userSubscribersLinkSection = document.createElement("div");
    userDiv.classList.add("card");
    userLinkSection.classList.add("card_data_section");
    userSubscribersLinkSection.classList.add("card_data_section");
    userLinkSection.appendChild(createUserLink(userInfo));
    userSubscribersLinkSection.appendChild(createUserSubscribersLink(userInfo.userId));
    userDiv.appendChild(userLinkSection);
    userDiv.appendChild(userSubscribersLinkSection);
    return userDiv;
}

function createUserLink(userInfo) {
    let userLink = document.createElement("a");
    userLink.classList.add("custom_link");
    userLink.setAttribute("href", `/users/${userInfo.userId}`);
    userLink.textContent = `${userInfo.userName} ${userInfo.userSurname}`;
    return userLink;
}

function createUserSubscribersLink(userId) {
    let subscribersLink = document.createElement("a");
    subscribersLink.classList.add("gray_link");
    subscribersLink.setAttribute("href", `/users/${userId}/subscribtions?filter=subscribers`);
    subscribersLink.textContent = `Подписчики`;
    return subscribersLink;
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

