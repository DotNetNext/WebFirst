jQuery editTable
=========

jQuery editTable is a very small jQuery Plugin (~1Kb gzipped) that fill the gap left by the missing of a default <strong>input field for data tables</strong>. jQuery editTable can be used both in ajax and/or HTTP POST contest and let you preset the title and number of columns or just let complete freedom to the user. You can even append custom behaviors to single column cells (ex. <strong>jQuery UI Datepicker</strong>). The only limit is your imagination! :)

To use it you just have to include jQuery and a copy of the plugin in your head or footer:

```html
<script type="text/javascript" src="http://code.jquery.com/jquery-latest.js"></script>
<script type="text/javascript" src="jquery.edittable.min.js"></script>
<link rel="stylesheet" href="jquery.edittable.min.css">
```

Now you can trigger editTable on any textarea or block element (ex. div, article, section ...). In case you trigger it on a textarea, its content will be used as JSON source for the table. If the textarea is inside a form, on submit, its content will be updated with the new JSON data. Otherwise, if you trigger it on a block element the table will be appended to the element itself (ajax).

```js
var mytable = $('#edittable').editTable({
    data: [['']],           // Fill the table with a js array (this is overridden by the textarea content if not empty)
    tableClass: 'inputtable',   // Table class, for styling
    jsonData: false,        // Fill the table with json data (this will override data property)
    headerCols: false,      // Fix columns number and names (array of column names)
    maxRows: 999,           // Max number of rows which can be added
    first_row: true,        // First row should be highlighted?
    row_template: false,    // An array of column types set in field_templates
    field_templates: false, // An array of custom field type objects

    // Validate fields
    validate_field: function (col_id, value, col_type, $element) {
        return true;
    }
});
```

There are of course many methods which can be used on the created table. Let's see...

```js
mytable.loadData(dataArray);    // Fill the table with js data
mytable.loadJsonData(jsonData); // Fill the table with JSON data
mytable.getData();              // Get a js array of the table data
mytable.getJsonData();          // Get JSON from the table data
mytable.reset();                // Reset the table to the initial set of data
mytable.isValidated()           // Check if the table pass validation set with validate_field
```

To define a <strong>custom field type</strong> object:

```js
[
    'checkbox' : {

        html: '<input type="checkbox">',     // Input type html

        // How to get the value from the custom input
        getValue: function (input) {
            return $(input).is(':checked');
        },

        // How to set the value of the custom input
        setValue: function (input, value) {
            if ( value ){
                return $(input).attr('checked', true);
            }
            return $(input).removeAttr('checked');
        }
    }
]
```

That's it, now give a look to [the examples](https://micc83.github.io/editTable/demo/) to understand how it works.

## Credits and contacts

ReStable has been made by [me](https://github.com/micc83). You can contact me at micc83@gmail.com or [twitter](https://twitter.com/Micc1983) for any issue or feauture request.
