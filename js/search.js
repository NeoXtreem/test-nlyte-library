async function search() {
  const query = $('#searchText').val();
  if (query.length > 0 && query.length < 3) {
    $('#errorMessage').text('Search text must be empty or at least 3 characters.');
    return false;
  }

  const book = $("input[type='radio'][name='book']:checked");
  if (book.length === 0) {
    $('#errorMessage').text('Please select a book.');
    return false;
  }

  try {
    $('#wordCounts').empty();
    $('#errorMessage').empty();

    $.each(await $.getJSON(`/api/books/${book.val()}?startsWith=${query}`), function (k, v) {
      $('#wordCounts').append(`<tr><td>${v.Word}</td><td> ${v.Count}</td></tr>`);
    });
  } catch (e) {
    $('#errorMessage').text(`Error Fetching Data: ${e}`);
    return false;
  }

  return true;
}
