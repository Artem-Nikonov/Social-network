document.addEventListener("DOMContentLoaded", DOMContentLoaded);
let subscribeBtn;

function DOMContentLoaded() {
    subscribeBtn = document.getElementById("subscribeBtn");
    if (subscribeBtn.textContent === "Подписаться")
        subscribeBtn.onclick = subscribeToUser;
    else
        subscribeBtn.onclick = unsubscribeFromUser
}

async function subscribeToUser() {
    try {
        const response = await fetch(`/subscribe/${pageId}?pageType=userPage`, {
            method: "POST",
        });

        if (response.ok) {
            console.log("подписка успешна");
            subscribeBtn.textContent = "Отписаться"
            subscribeBtn.onclick = unsubscribeFromUser;
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}

async function unsubscribeFromUser() {
    try {
        const response = await fetch(`/unsubscribe/${pageId}?pageType=userPage`, {
            method: "DELETE",
        });

        if (response.ok) {
            console.log("отписка успешна");
            subscribeBtn.textContent = "Подписаться"
            subscribeBtn.onclick = subscribeToUser;
        }
        else {
            console.error("Произошла ошибка при выполнении запроса");
        }
    }
    catch (error) {
        console.error("Ошибка:", error.message);
    }
}