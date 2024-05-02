document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getGroups);
let userId = document.getElementById("PageId").getAttribute("data");
let pageId = 1;
let groupsContainer;
let addLoadGroupsBtn;

function DOMContentLoaded() {
    groupsContainer = document.getElementById("pagesContainer");
    addLoadGroupsBtn = document.getElementById("AddLoadDataBtn");
    addLoadGroupsBtn.addEventListener("click", getGroups);
}


async function getGroups() {
    try {
        const response = await fetch(`/users/${userId}/groups/list?page=${pageId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Группы получены");
            groupsData = await response.json()
            console.log(groupsData)
            for (let group of groupsData.groups) {
                groupsContainer.appendChild(createGroupDiv(group));
            }
            lastPageHandler(groupsData.meta);
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function createGroupDiv(groupInfo) {
    let groupDiv = document.createElement("div");
    let groupLinkSection = document.createElement("div");
    let groupSubscribersLinkSection = document.createElement("div");
    groupDiv.classList.add("card");
    groupLinkSection.classList.add("card_data_section");
    groupSubscribersLinkSection.classList.add("card_data_section");
    groupLinkSection.appendChild(createGroupLink(groupInfo));
    groupSubscribersLinkSection.appendChild(createGroupSubscribersLink(groupInfo.groupId));
    groupDiv.appendChild(groupLinkSection);
    groupDiv.appendChild(groupSubscribersLinkSection);
    return groupDiv;
}

function createGroupLink(groupInfo) {
    let groupLink = document.createElement("a");
    groupLink.classList.add("custom_link");
    groupLink.setAttribute("href", `/groups/${groupInfo.groupId}`);
    groupLink.textContent = `${groupInfo.groupName}`;
    return groupLink;
}

function createGroupSubscribersLink(groupId) {
    let subscribersLink = document.createElement("a");
    subscribersLink.classList.add("gray_link");
    subscribersLink.setAttribute("href", `/groups/${groupId}/subscribers`);
    subscribersLink.textContent = `Подписчики`;
    return subscribersLink;
}

function lastPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadGroupsBtn.style.display = "none";
    }
    else {
        addLoadGroupsBtn.style.display = "inline-block";
    }
    pageId = pageMetaData.pageId + 1;
}