// import Something from "./another-js-file.js";

class App
{
  async go() {
    const bookResults = document.querySelector('.books');
    bookResults.innerHTML = '<div>Loading...</div>';

    let booksHtml = '';
    (await (await fetch('/api/books')).json()).forEach(book => {
      booksHtml += `<div><input type="radio" value="${book.Id}" name="book"><label for="${book.Id}">${book.Title}</label></div>`;
    });

    bookResults.innerHTML = booksHtml;
  }
}

await new App().go();