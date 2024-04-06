document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getPosts);
let pageId = document.getElementById("PageId").getAttribute("data");
let isOwner = document.getElementById("IsOwner").getAttribute("data-owner") 
let pageName = document.getElementById("userFullName").textContent;

let startPostId = 0;
let postTextArea;
let publishBtn;
let postsContainer;
let addLoadPostsBtn;

function DOMContentLoaded()
{
    console.log(isOwner)
    if (isOwner == "True") {
        postTextArea = document.getElementById("postContent");
        publishBtn = document.getElementById("publishBtn");
        publishBtn.addEventListener("click", publishBtnClick)
    }
    postsContainer = document.getElementById("postsContainer");
    addLoadPostsBtn = document.getElementById("AddLoadPostsBtn");
    addLoadPostsBtn.addEventListener("click", getPosts);
}

async function publishBtnClick() {
    const postContent = postTextArea.value;
    if (postContent.length < 1) return;
    const postData = {
        Content: postContent
    };
    try
    {
        const response = await fetch("/userPosts/create", {
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
        if (response.ok)
        {
            console.log("Пост успешно добавлен");
            postTextArea.value = "";
            const postInfo = await response.json();
            const post = createPost(postInfo);
            let lastPostInCintainer = postsContainer.firstChild;
            postsContainer.insertBefore(post,lastPostInCintainer)
        }
        else
        {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error)
    {
        console.error("Ошибка:", error.message);
    }
};

async function getPosts() {
    try {
        const response = await fetch(`/userPosts/${pageId}?startPostId=${startPostId}`, {
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
    dataDiv.appendChild(createMetaDataDiv(pageName));
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

function createTimeSpanDiv(postInfo) {
    let timeSpanDiv = document.createElement("div");
    timeSpanDiv.classList.add("post_data_section");
    timeSpanDiv.appendChild(createTimeSpan(postInfo.creationDate));
    return timeSpanDiv;
}

function createMetaDataDiv(name) {
    let metaDataDiv = document.createElement("div");
    metaDataDiv.classList.add("post_data_section");
    let autorLink = document.createElement("a");
    autorLink.classList.add("custom_link");
    autorLink.setAttribute("href", `/users/${pageId}`);
    autorLink.textContent = name;
    metaDataDiv.appendChild(autorLink)
    if (isOwner === "True")
        metaDataDiv.appendChild(createDeleteButton());
    return metaDataDiv;
}

function createTimeSpan(time) {
    let timeSpan = document.createElement("span");
    timeSpan.textContent = time;
    return timeSpan;
}

function createDeleteButton() {
    let delBtn = document.createElement("button");
    delBtn.classList.add("link_btn");
    delBtn.textContent = "Удалить";
    delBtn.addEventListener("click", delBtnClick);
    return delBtn;
}

async function delBtnClick() {
    const postId = this.parentNode.parentNode.getAttribute("data-post-id");
    try {
        const response = await fetch(`/userPosts/delete/${postId}`, {
            method: "PATCH"
        });

        if (response.ok) {
            console.log("Пост удален.");
            let post = this.parentNode.parentNode.parentNode;
            post.remove();
        }
        else {
            console.error(await response.text());
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