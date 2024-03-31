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
            if (postsData.meta.isLastPage)
                addLoadPostsBtn.style.display = "none";
            startPostId = postsData.meta.lastPostId - 1;
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
    postDiv.appendChild(createPostDataDiv(postInfo));
    postDiv.appendChild(createPostContentDiv(postInfo));
    return postDiv;
}

function createPostDataDiv(postInfo) {
    let dataDiv = document.createElement("div");
    dataDiv.appendChild(createAutorLink(postInfo.userId, pageName));
    dataDiv.appendChild(createTimeSpan(postInfo.creationDate));
    return dataDiv;
}

function createPostContentDiv(postInfo) {
    let contentDiv = document.createElement("div");
    let content = document.createElement("pre");
    content.textContent = postInfo.content;
    contentDiv.appendChild(content)
    return contentDiv;
}

function createAutorLink(id, name) {
    let autorLink = document.createElement("a");
    autorLink.setAttribute("href", id);
    autorLink.textContent = name;
    return autorLink;
}

function createTimeSpan(time) {
    let timeSpan = document.createElement("span");
    timeSpan.textContent = time;
    return timeSpan;
}