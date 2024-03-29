document.addEventListener("DOMContentLoaded", DOMContentLoaded);
document.addEventListener("DOMContentLoaded", getPosts);
const IdSpan = document.getElementById("PageId");
var pageId = IdSpan.getAttribute("data");
var partId=1;
function DOMContentLoaded()
{
    const publishBtn = document.getElementById("publishBtn");
    publishBtn.addEventListener("click", publishBtnClick)
}

async function publishBtnClick() {
    var postTextArea = document.getElementById("postContent");
    const postContent = postTextArea.value;

    const postData = {
        Content: postContent
    };

    try
    {
        const response = await fetch("/userPost/add", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(postData)
        });

        if (response.ok)
        {
            console.log("Пост успешно добавлен");
            postTextArea.value = "";
            // Дополнительные действия при успешном добавлении поста
        }
        else
        {
            console.error("Произошла ошибка при выполнении запроса");
            // Дополнительные действия в случае ошибки
        }
    }
    catch (error)
    {
        console.error("Ошибка:", error.message);
        // Дополнительные действия при возникновении ошибки
    }
};

async function getPosts() {
    let posts;
    try {
        const response = await fetch(`/userPost/${pageId}?partId=${partId}`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

        if (response.ok) {
            console.log("Посты получены");
            posts = await response.json()
            console.log(posts)
            pageId++;
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}


