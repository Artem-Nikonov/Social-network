document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getPosts);
let pageId = document.getElementById("PageId").getAttribute("data");
let visitorId = document.getElementById("VisitorId").getAttribute("data");
let isOwner = document.getElementById("IsOwner").getAttribute("data-owner")
let pageName = document.getElementById("groupName").textContent;

let startPostId = 0;
let postTextArea;
let publishBtn;
let postsContainer;
let addLoadPostsBtn;

function DOMContentLoaded() {
    console.log(isOwner)
    postTextArea = document.getElementById("postContent");
    publishBtn = document.getElementById("publishBtn");
    publishBtn.addEventListener("click", publishBtnClick);

    postsContainer = document.getElementById("postsContainer");
    addLoadPostsBtn = document.getElementById("AddLoadPostsBtn");
    addLoadPostsBtn.addEventListener("click", getPosts);
}

async function publishBtnClick() {
    const postContent = postTextArea.value;
    if (postContent.length < 1) return;
    const postData = {
        Content: postContent,
        GroupId: pageId
    };
    console.log(postData)
    try {
        const response = await fetch("/posts/create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(postData)
        });

        if (response.redirected) {
            console.log(response.url);
            return;
        }
        if (response.ok) {
            console.log("Пост успешно добавлен");
            postTextArea.value = "";
            const postInfo = await response.json();
            const post = createPost(postInfo);
            let lastPostInCintainer = postsContainer.firstChild;
            postsContainer.insertBefore(post, lastPostInCintainer)
        }
        else {
            let statusText = await response.text();
            showError(statusText);
            console.log(statusText)
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
};

async function getPosts() {
    try {
        const response = await fetch(`/pagePosts/${pageId}?pageType=group&startPostId=${startPostId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Посты получены");
            postsData = await response.json()
            console.log(postsData)
            for (let post of postsData.posts) {
                postsContainer.appendChild(createPost(post));
            }
            lastPageHandler(postsData.meta)
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function createPost(postInfo) {
    let postDiv = document.createElement("div");
    postDiv.classList.add("post");
    postDiv.appendChild(createPostDataDiv(postInfo));
    postDiv.appendChild(createPostContentDiv(postInfo));
    return postDiv;
}

function createPostDataDiv(postInfo) {
    let dataDiv = document.createElement("div");
    dataDiv.classList.add("post_data");
    dataDiv.setAttribute("data-post-id", postInfo.postId)
    dataDiv.setAttribute("data-autor-id", postInfo.userId)
    dataDiv.setAttribute("data-group-id", postInfo.groupId)
    dataDiv.appendChild(createMetaDataDiv(postInfo));
    dataDiv.appendChild(createTimeSpanDiv(postInfo));
    return dataDiv;
}

function createPostContentDiv(postInfo) {
    let contentDiv = document.createElement("div");
    contentDiv.classList.add("post_content");
    let content = document.createElement("pre");
    content.textContent = postInfo.content;
    contentDiv.appendChild(content)
    return contentDiv;
}

function createMetaDataDiv(postInfo) {
    let metaDataDiv = document.createElement("div");
    metaDataDiv.classList.add("post_data_section");
    let mainMeta = document.createElement("div");
    mainMeta.classList.add("post_data_link_container");
    mainMeta.appendChild(createGroupLink(pageId, pageName));
    mainMeta.appendChild(createAutorLink(postInfo.userId));
    metaDataDiv.appendChild(mainMeta);
    if (isOwner === "True" || postInfo.userId == visitorId)
        metaDataDiv.appendChild(createDeleteButton());
    return metaDataDiv;
}

function createTimeSpanDiv(postInfo) {
    let timeSpanDiv = document.createElement("div");
    timeSpanDiv.classList.add("post_data_section");
    timeSpanDiv.appendChild(createTimeSpan(postInfo.creationDate));
    return timeSpanDiv;
}

function createGroupLink(groupId, groupName) {
    let groupLink = document.createElement("a");
    groupLink.classList.add("custom_link");
    groupLink.setAttribute("href", `/groups/${groupId}`);
    groupLink.textContent = groupName;
    return groupLink;
}

function createAutorLink(autorId) {
    let autorLink = document.createElement("a");
    autorLink.classList.add("custom_link");
    autorLink.setAttribute("href", `/users/${autorId}`);
    autorLink.textContent = "Автор";
    return autorLink;
}

function createDeleteButton() {
    let delBtn = document.createElement("button");
    delBtn.classList.add("link_btn");
    delBtn.textContent = "Удалить";
    delBtn.addEventListener("click", delBtnClick);
    return delBtn;
}

function createTimeSpan(time) {
    let timeSpan = document.createElement("span");
    timeSpan.classList.add("time_span");
    timeSpan.textContent = time;
    return timeSpan;
}

async function delBtnClick() {
    const postId = this.parentNode.parentNode.getAttribute("data-post-id");
    try {
        const response = await fetch(`/pagePosts/delete/${postId}?pageType=group`, {
            method: "PATCH"
        });

        if (response.ok) {
            console.log("Пост удален.");
            let post = this.parentNode.parentNode.parentNode;
            post.remove();
        }
        else {
            showError(response.statusText);
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

function lastPageHandler(pageMetaData) {
    if (pageMetaData.isLastPage) {
        addLoadPostsBtn.style.display = "none";
    }
    else {
        addLoadPostsBtn.style.display = "inline-block";
    }
    startPostId = postsData.meta.lastPostId - 1;
}
function showError(message) {
    var errorWindow = document.getElementById("errorWindow");
    var errorMessage = document.getElementById("errorMessage");

    errorMessage.textContent = message;
    errorWindow.classList.add("active");

    setTimeout(function () {
        errorWindow.classList.remove("active");
        errorWindow.classList.add("fade-out");
        setTimeout(function () {
            errorWindow.classList.remove("fade-out");
        }, 500); // После завершения анимации исчезновения
    }, 5000); // После 5 секунд исчезает
}