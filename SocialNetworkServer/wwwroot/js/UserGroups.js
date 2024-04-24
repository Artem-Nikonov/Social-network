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
    groupDiv.appendChild(createGroupLink(groupInfo));
    return groupDiv;
}

function createGroupLink(groupInfo) {
    let groupLink = document.createElement("a");
    groupLink.classList.add("custom_link");
    groupLink.setAttribute("href", `/groups/${groupInfo.groupId}`);
    groupLink.textContent = `${groupInfo.groupName}`;
    return groupLink;
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