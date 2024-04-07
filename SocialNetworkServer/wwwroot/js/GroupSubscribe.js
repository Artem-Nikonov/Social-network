document.addEventListener("DOMContentLoaded", DOMContentLoaded);
let pageId = document.getElementById("PageId").getAttribute("data");
let subscribeBtn;

function DOMContentLoaded() {
    subscribeBtn = document.getElementById("subscribeBtn");
    if (subscribeBtn.textContent === "Подписаться")
        subscribeBtn.onclick = subscribeToGroup;
    else
        subscribeBtn.onclick = unsubscribeFromGroup
}

async function subscribeToGroup() {
    try {
        const response = await fetch(`/subscribe/${pageId}?pageType=group`, {
            method: "POST",
        });

        if (response.ok) {
            console.log("подписка успешна");
            subscribeBtn.textContent = "Отписаться"
            subscribeBtn.onclick = unsubscribeFromGroup;
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

async function unsubscribeFromGroup() {
    try {
        const response = await fetch(`/unsubscribe/${pageId}?pageType=group`, {
            method: "DELETE",
        });

        if (response.ok) {
            console.log("подписка успешна");
            subscribeBtn.textContent = "Подписаться"
            subscribeBtn.onclick = subscribeToGroup;
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}