// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Lấy ID từ URL
const params = new URLSearchParams(window.location.search);
const bookId = params.get('id');

// Tìm sách theo id
const selectedBook = books.find(book => book.id === bookId);

// Gán thông tin sách vào trang
if (selectedBook) {
    document.getElementById('bookTitle').textContent = selectedBook.title;
    document.getElementById('bookCategory').textContent = "Thể loại: " + selectedBook.category;
    document.getElementById('bookImage').src = selectedBook.image;
    document.getElementById('bookImage').alt = selectedBook.title;
    document.getElementById('bookDescription').textContent = selectedBook.description;
} else {
    document.querySelector('.book-detail').innerHTML = '<h3>Sách không tồn tại!</h3>';
}
