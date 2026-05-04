const express = require('express');
const cors = require('cors');  // Подключаем CORS
const multer = require('multer');
const path = require('path');
const app = express();
const port = 5000;

app.use(cors()); 
app.use(express.static(path.join(__dirname, '..', 'public')));

// Настройка хранения загруженных файлов
const storage = multer.diskStorage({
  destination: (req, file, cb) => {
    cb(null, 'uploads/'); // Папка для хранения загруженных треков
  },
  filename: (req, file, cb) => {
    cb(null, Date.now() + path.extname(file.originalname)); // Уникальное имя файла
  }
});

const upload = multer({ storage: storage });

// Настроим папку для статики (например, изображения или JS)
app.use(express.static('public'));
app.use(express.json());

// Данные для вкладок
const trackData = {
  lyrics: `
    И когда они выходят из рамы,<br>
    Улыбаясь, поправляют прически,<br>
    Я для каждого в просторной прихожей<br>
    Оставляю зеркала и расчески.<br>
    И когда они гуляют по дому,<br>
    И смеются, и играют словами,<br>
    Я для них сдвигаю мягкие кресла<br>
    И растапливаю печку дровами.<br><br>

    А когда они листают альбомы,<br>
    Удивляясь, головами качают,<br>
    Я несу на блюде синие сливы,<br>
    Наливаю золотистого чаю.<br>
    А потом ныряю в кресло-качалку<br>
    И ловлю буквально каждую фразу!<br>
    Жаль, они меня не могут увидеть<br>
    Даже мельком, даже краешком глаза.<br><br>

    Их истории давно устарели,<br>
    Платья тоже быстро вышли из моды,<br>
    Но остались те же ясные лица,<br>
    И во взгляде будто больше свободы.<br>
    Я люблю их, вот таких старомодных,<br>
    И заглядываю в зеркальце, ну же,<br>
    И ищу в себе портретное сходство:<br>
    Нос – ничей, зато «фамильные» уши!<br><br>

    И стараюсь быть такой же свободной,<br>
    Подчиняюсь только голосу крови!<br>
    У меня такой же крепкий характер,<br>
    И такие, как у бабушки, брови…<br>
    Иногда я расчехляю гитару<br>
    И наигрываю: до-ре-ми-соль-фа…<br>
    И тогда они меня вспоминают,<br>
    Я для них все та же девочка в гольфах.<br><br>

    Между нами встало время стеною:<br>
    Ни просвета, ни двери, ни окошка,<br>
    Между нами будто тонкая пленка,<br>
    Сквозь которую курсирует кошка.<br>
    А потом упрямо близится полночь,<br>
    И тогда они уходят обратно.<br>
    Я свечу им фонарем на тропинку,<br>
    Как же мне теперь без них, непонятно.<br><br>

    Унесла их от меня электричка,<br>
    Взмыла в небо желтой искоркой плавной,<br>
    И запели в синих травах цикады,<br>
    Это было так давно, так недавно.<br>
    Протираю золоченую раму,<br>
    Помоги им в долгих странствиях, Боже!<br>
    Ведь когда тебе совсем одиноко,<br>
    Так приятно вспоминать о хорошем.
  `,
  queue: [
    {
      title: "Твоими нитями",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },
    {
      title: "Мне бы хотелось",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },
{
      title: "Один на один с пустотой",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },

    {
      title: "Новогодняя",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },

    {
      title: "Грузия",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },

    {
      title: "Шли они",
      artist: "Зоя Ященко, Белая Гвардия · Венеция",
      image: "/public/images/semeinoe_photo_zoya_yachenko.jpg"
    },
  
  ]
};

// API для получения данных вкладки
app.get('/api/tab/:tabId', (req, res) => {
  const tabId = req.params.tabId;

  if (tabId === 'lyrics') {
    res.json({ content: trackData.lyrics });
  } else if (tabId === 'queue') {
    res.json({ content: trackData.queue });
  } else {
    res.status(404).json({ error: 'Tab not found' });
  }
});

// Обработчик для загрузки трека
app.post('/upload-track', upload.single('track'), (req, res) => {
  if (!req.file) {
    return res.status(400).json({ message: 'Нет файла для загрузки' });
  }

  // После успешной загрузки
  res.json({ message: 'Трек успешно загружен!' });
});

// Обработчик для поиска
app.get('/search', (req, res) => {
  const { query } = req.query;

  // Здесь можно добавить реальную логику поиска в базе данных
  const suggestions = [
    'Трек 1',
    'Трек 2',
    'Трек 3'
  ];

  // Ищем совпадения
  const filteredSuggestions = suggestions.filter(suggestion => suggestion.toLowerCase().includes(query.toLowerCase()));

  res.json({ suggestions: filteredSuggestions });
});

// Запуск сервера
app.listen(port, () => {
  console.log(`Server running at http://localhost:${port}`);
});