'use strict';

function toggleRow(rowToToggle, buttonThatCalled) {
  $('#RowID' + rowToToggle).toggle();

  $('#menucolumn, #ColWide, #ColTwoPadder').css('height', 'auto');

  var $button = $(buttonThatCalled);

  if ($button.hasClass("expandBtn")) {
    var $icon = $button.children("i");

    if ($icon.hasClass("fa-plus-circle")) {
      $icon.removeClass("fa-plus-circle");
      $icon.addClass("fa-minus-circle");
    } else {
      $icon.removeClass("fa-minus-circle");
      $icon.addClass("fa-plus-circle");
    }

  } else if (buttonThatCalled.value == "+") { buttonThatCalled.value = "-"; } else { buttonThatCalled.value = "+"; }

  if (equalHeight) {
    equalHeight($("#ColWide, #menucolumn, #ColTwoPadder"));
  }
}

function toggleExpandAllTable(buttonThatCalled) {
  var $button = $(buttonThatCalled);

  if ($button.hasClass("expandBtn")) {
    var $icon = $button.children("i");

    if ($icon.hasClass("fa-plus-circle")) {
      expandAllTable();
      $icon.removeClass("fa-plus-circle");
      $icon.addClass("fa-minus-circle");
    } else {
      hideAllTable();
      $icon.removeClass("fa-minus-circle");
      $icon.addClass("fa-plus-circle");
    }

  } else if (buttonThatCalled.value == "+") {
    expandAllTable();
    buttonThatCalled.value = "-";
  } else {
    hideAllTable();
    buttonThatCalled.value = "+";
  }
}

function expandAllTable() {
  $('#menucolumn, #ColWide, #ColTwoPadder').css('height', 'auto');
  $('.WebGrid .expandedRow, .expandedRow').show();
  $('.WebGrid .fa-plus-circle').removeClass("fa-plus-circle").addClass("fa-minus-circle");

  if (equalHeight) {
    equalHeight($("#ColWide, #menucolumn, #ColTwoPadder"));
  }

}

function hideAllTable() {
  $('#menucolumn, #ColWide, #ColTwoPadder').css('height', 'auto');
  $('.WebGrid .expandedRow, .expandedRow').hide();
  $('.WebGrid .fa-minus-circle').removeClass("fa-minus-circle").addClass("fa-plus-circle");

  if (equalHeight) {
    equalHeight($("#ColWide, #menucolumn, #ColTwoPadder"));
  }
}

function initialiseWebGrid() {
  //Set the width of the table headers, so it doesn't resize when rows are expanded

  var hasWrapper = false;

  var $table = $(".WebGrid");

  var $parent = $table.parent();

  var css = {}

  if ($parent.hasClass("WebGridWrapper")) {
    hasWrapper = true;
    css.position = $table.css("position")
    css.left = $table.css("left")
    $table.detach().appendTo("body");

    $table.css({
      position: "absolute",
      left: -9999
    });
  }

  $table.find("tbody > tr > th").each(function () {
    var $header = $(this);
    var $table = $header.closest("table");

    if ($header.data("sort") !== false && $table.data("sort") !== false) {
      $header.append("<i class='sorting-caret fa fa-sort'></i>");

      if ($header.data("sort") === undefined) {
        var columnIndex = $header.index();

        var $firstRow = $table.find("tbody > tr").eq(1);

        if ($firstRow.length) {
          var $cell = $firstRow.children("td:nth-child(" + (columnIndex + 1) + ")");

          var html = $cell.html();

          if (!html) {
            return;
          }

          var data = html.trim();

          if (data.substring(0, 1) == "£") {
            $header.data("sort", "currency");
          }

          if (!isNaN(filterInt(data))) {
            $header.data("sort", "numeric");
          }

          if (getDate(data)) {
            $header.data("sort", "date");
          }

          if (data.substring(0, 6) == "<input" || data.substring(0, 7) == "<button" || data.substring(0, 2) == "<i" || data.substring(0, 2) == "<a") {
            $header.data("sort", false);
            $header.children("i").detach();
            $header.addClass("nosort");
          }
        }
      }
    }
    else {
      $header.addClass("nosort");
    }

    $header.css("width", $header.width());
  });

  addSorting($table);

  if (hasWrapper) {
    $table.css(css);
    $table.detach().appendTo($parent);
  }

  function addSorting($table) {

    $table.on("click", "tbody > tr > th", function (event) {
      event.stopPropagation();

      var $header = $(this);

      var $tbody = $header.closest("tbody");

      var $table = $tbody.parent("table");

      //do not sort if either the table or header has sorting disabled
      if ($header.data("sort") === false || $table.data("sort") === false) {
        return;
      }

      var $rows = $tbody.children("tr");

      var data = [];

      var columnIndex = $header.index();

      //Find the data from each row
      $rows.each(function (index, row) {

        if (index == 0) {
          return 0;
        }

        var $row = $(row);

        if (!$row.is(":visible") && index > 1) {
          data[index - 1] = {
            index: index,
            data: data[index - 2].data
          };
        }
        else {
          data[index - 1] = {
            index: index,
            data: $row.children("td").eq(columnIndex).html().trim()
          };
        }

      });

      var $headers = $rows.eq(0).find("th");

      resetCarets($headers, columnIndex);

      var sortFunction = getSortFunction($header.data("sort"));

      //sort in order or in inverse order depending on the current situation
      if ($header.data("sortOrder") == "sorted") {
        data.sort(sortingFunctions.inverse(sortFunction));

        $header.data("sortOrder", "inverse");

        var $caret = $header.find(".sorting-caret");

        if ($caret.length) {
          $caret.removeClass("fa-caret-down");
          $caret.removeClass("fa-sort");
          $caret.addClass("fa-caret-up");
        }
        else {
          $header.append("<i class='sorting-caret fa fa-caret-up'></i>");
        }
      }
      else {
        data.sort(sortFunction);
        $header.data("sortOrder", "sorted");

        var $caret = $header.find(".sorting-caret");

        if ($caret.length) {
          $caret.removeClass("fa-caret-up");
          $caret.removeClass("fa-sort");
          $caret.addClass("fa-caret-down");
        }
        else {
          $header.append("<i class='sorting-caret fa fa-caret-down'></i>");
        }
      }

      $rows.detach();

      $tbody.append($rows.eq(0));

      for (var i = 0; i < data.length; i++) {
        $tbody.append($rows.eq(data[i].index));
      }
    });

    function resetCarets($headers, columnIndex) {
      $headers.each(function (index, header) {

        var $header = $(header);

        if ($header.index() == columnIndex || $header.data("sort") === false) {
          return;
        }

        var $caret = $header.find(".sorting-caret");

        if ($caret.length) {
          $caret.removeClass("fa-caret-down");
          $caret.removeClass("fa-caret-up");
          $caret.addClass("fa-sort");
        } else {
          $header.append("<i class='sorting-caret fa fa-sort'></i>");
        }
      });
    }

    function getSortFunction(sortType) {
      switch (sortType) {
        case "currency":
          return sortingFunctions.currency;
        case "numeric":
          return sortingFunctions.numeric;
        case "date":
          return sortingFunctions.date;
        case "text":
        default:
          return sortingFunctions.text;
      }
    }

    var sortingFunctions = new SortingFunctions();

    function SortingFunctions() {
      var sortingFunctions = {};

      sortingFunctions.text = function (a, b) {
        return sort(a.data, b.data);
      }

      sortingFunctions.inverse = function (sortFunction) {

        return function (a, b) {
          var sortResult = sortFunction(a, b);

          if (sortResult == 1) {
            return -1;
          }
          else if (sortResult == -1) {
            return 1;
          }
          else {
            return 0;
          }
        }
      }

      sortingFunctions.numeric = function (a, b) {
        var numA = parseFloat(a.data);
        var numB = parseFloat(b.data);

        if (isNaN(numA) || isNaN(numB)) {
          return sort(a.data, b.data);
        }
        else {
          return sort(numA, numB);
        }
      }

      sortingFunctions.currency = function (a, b) {
        var currencyA = parseFloat(a.data.substring(1));
        var currencyB = parseFloat(b.data.substring(1));

        if (isNaN(currencyA) || isNaN(currencyB)) {
          return sort(a.data, b.data);
        }
        else {
          return sort(currencyA, currencyB);
        }
      }

      sortingFunctions.date = function (a, b) {
        var dateA = getDate(a.data);
        var dateB = getDate(b.data);

        if (!dateA || !dateB) {
          return sort(a.data, b.data);
        }
        else {
          return sort(dateA.getTime(), dateB.getTime());
        }
      }

      return sortingFunctions;

      function sort(a, b) {
        if (a < b) {
          return -1;
        }
        else if (a > b) {
          return 1;
        }
        else {
          return 0;
        }
      }
    }
  }

  function getDate(data) {
    var dateParts = data.split("/");

    if (dateParts.length == 3) {
      if (isNaN(filterInt(dateParts[2]))) {
        var yearTimeParts = dateParts[2].trim().split(" ");

        if (yearTimeParts.length == 2) {
          var timeParts = yearTimeParts[1].split(":");

          if (timeParts.length == 2) {
            return new Date(yearTimeParts[0], dateParts[1] - 1, dateParts[0], timeParts[0], timeParts[1]);
          }
          else {
            return null;
          }
        } else {
          return null;
        }

      } else {
        return new Date(dateParts[2], dateParts[1] - 1, dateParts[0]);
      }
    }
    else {
      return null;
    }

  }

  function filterInt(value) {
    if (/^(\-|\+)?([0-9]+|Infinity)$/.test(value))
      return Number(value);
    return NaN;
  }
}

$(document).ready(function () {
  initialiseWebGrid();
});