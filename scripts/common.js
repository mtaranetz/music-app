const API_BASE = "https://music-app-server-117v.onrender.com/api";

function goToPage(page) {
  const isRoot = location.pathname.endsWith("/index.html") || location.pathname === "/";
  const prefix = isRoot ? "./" : "../";

  const routes = {
    "Главная": `${prefix}index.html`,
    "Новинки": `${prefix}pages/new_albums.html`,
    "Артисты": `${prefix}pages/artists.html`,
    "Добавить трек": `${prefix}pages/add_track.html`
  };

  if (routes[page]) location.href = routes[page];
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

  miniPlayer.addEventListener("click", e => {
    if (!e.target.closest(".action") && !e.target.closest(".progress")) {
      const isRoot = location.pathname.endsWith("/index.html") || location.pathname === "/";
      const prefix = isRoot ? "./" : "../";
      location.href = `${prefix}pages/track_cards/track-card_01.html?track=1`;
    }
  });

  if (actionButton) {
    actionButton.addEventListener("click", e => {
      e.stopPropagation();
      console.log("Работа с треком");
    });
  }

  if (progress) {
    progress.addEventListener("click", e => {
      e.stopPropagation();
      const bar = progress.querySelector(".progress-bar");
      const rect = progress.getBoundingClientRect();
      const ratio = Math.min(Math.max((e.clientX - rect.left) / rect.width, 0), 1);
      if (bar) bar.style.width = `${ratio * 100}%`;
    });
  }
}

function initSearch() {
  const searchInput = document.getElementById("searchInput");
  const suggestions = document.getElementById("suggestions");

  if (!searchInput || !suggestions) return;

  searchInput.addEventListener("input", async () => {
    const query = searchInput.value.trim();

    if (query.length < 3) {
      suggestions.style.display = "none";
      return;
    }

    try {
      const response = await fetch(`${API_BASE}/search/search?query=${encodeURIComponent(query)}`);
      const data = await response.json();

      renderSuggestions(data.suggestions || []);
    } catch (error) {
      console.error(error);
      suggestions.style.display = "none";
    }
  });

  function renderSuggestions(items) {
    suggestions.innerHTML = "";

    if (!items.length) {
      suggestions.style.display = "none";
      return;
    }

    items.forEach(item => {
      const div = document.createElement("div");
      div.className = "suggestion-item";
      div.textContent = `${item.title} — ${item.artist}`;

      div.addEventListener("click", () => {
        location.href = item.url;
      });

      suggestions.appendChild(div);
    });

    suggestions.style.display = "block";
  }
}

document.addEventListener("DOMContentLoaded", () => {
  initSidebar();
  initMiniPlayer();
  initSearch();
});