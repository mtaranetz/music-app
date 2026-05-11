const API_BASE = "https://music-app-server-117v.onrender.com/api";

function isRootPage() {
  return (
    location.pathname === "/" ||
    location.pathname.endsWith("/index.html")
  );
}

function getBasePath() {
  const path = location.pathname;

  if (path.endsWith("/index.html") || path === "/") {
    return "./";
  }

  if (path.includes("/pages/collections/") || path.includes("/pages/track_cards/")) {
    return "../../";
  }

  return "../";
}

function goToPage(page) {
  const base = getBasePath();

  const routes = {
    "Главная": `${base}index.html`,
    "Новинки": `${base}pages/new_albums.html`,
    "Артисты": `${base}pages/artists.html`,
    "Добавить трек": `${base}pages/add_track.html`
  };

  if (routes[page]) {
    location.href = routes[page];
  }
}

function initSidebar() {
  document.querySelectorAll(".sb-btn").forEach(btn => {
    btn.addEventListener("click", () => {
      goToPage(btn.textContent.trim());
    });
  });
}

function initMiniPlayer() {
  const miniPlayer = document.getElementById("miniPlayer");
  if (!miniPlayer) return;

  const actionButton = miniPlayer.querySelector(".action");
  const progress = miniPlayer.querySelector(".progress");
  const progressBar = miniPlayer.querySelector(".progress-bar");

  miniPlayer.addEventListener("click", e => {
    if (!e.target.closest(".action") && !e.target.closest(".progress")) {
      location.href = `${getBasePath()}pages/track_cards/track-card_01.html?track=1`;
    }
  });

  if (actionButton) {
    actionButton.addEventListener("click", e => {
      e.stopPropagation();
      console.log("Работа с треком");
    });
  }

  if (progress && progressBar) {
    progress.addEventListener("click", e => {
      e.stopPropagation();

      const rect = progress.getBoundingClientRect();
      const ratio = Math.min(
        Math.max((e.clientX - rect.left) / rect.width, 0),
        1
      );

      progressBar.style.width = `${ratio * 100}%`;
    });
  }
}

function initSearch() {
  const searchInput = document.getElementById("searchInput");
  const suggestions = document.getElementById("suggestions");

  if (!searchInput || !suggestions) {
    console.warn("Поиск не найден на странице");
    return;
  }

  searchInput.addEventListener("input", async () => {
    const query = searchInput.value.trim();

    if (query.length < 3) {
      suggestions.innerHTML = "";
      suggestions.style.display = "none";
      return;
    }

    try {
      const url = `${API_BASE}/search/search?query=${encodeURIComponent(query)}`;
      console.log("Search request:", url);

      const response = await fetch(url);

      if (!response.ok) {
        throw new Error(`Ошибка поиска: ${response.status}`);
      }

      const data = await response.json();
      console.log("Search response:", data);

      renderSuggestions(data.suggestions || []);
    } catch (error) {
      console.error("Ошибка при получении подсказок:", error);
      suggestions.innerHTML = "";
      suggestions.style.display = "none";
    }
  });

  document.addEventListener("click", e => {
    if (!e.target.closest(".search")) {
      suggestions.style.display = "none";
    }
  });
}

function renderSuggestions(items) {
  const suggestions = document.getElementById("suggestions");
  if (!suggestions) return;

  suggestions.innerHTML = "";

  if (!items.length) {
    suggestions.style.display = "none";
    return;
  }

  items.forEach(item => {
    const div = document.createElement("div");
    div.className = "suggestion-item";

    if (typeof item === "string") {
      div.textContent = item;
    } else {
      div.textContent = `${item.title || "Без названия"} — ${item.artist || "Неизвестный артист"}`;
    }

    div.addEventListener("click", () => {
      if (typeof item === "string") {
        searchInput.value = item;
        suggestions.style.display = "none";
        return;
      }

      location.href = `${getBasePath()}pages/track_cards/track-card_01.html?track=${item.id}`;
    });

    suggestions.appendChild(div);
  });

  suggestions.style.display = "block";
}

document.addEventListener("DOMContentLoaded", () => {
  initSidebar();
  initMiniPlayer();
  initSearch();
});