
## 1.2.1 Updates
- Filter is fixed & updated.
- icontains.js dependency is deprecated. 

# ComboTree jQuery Plugin v 1.2.1
ComboTree is a jQuery Plugin which is a combobox item within tree structured data list and multiple/single selection options and more. It has been developed to manage large amount of choices in a combobox and multi selection feature. 


## Features:
- Tree structured data list in combobox dropdown menu
- Multiple & Single selection
- Cascade selection (for multiple mode)
- Returns selected item(s) as title or id array
- Filtering (for multiple mode)
- Consumes JSON source
- Key controls are available for both selection and filter inputs.

 
## Dependencies:
- jQuery
 
## Configurations:
- **isMultiple**: *{true/false} | default: false* | decide if it is multiple selection behaviour or single
- **cascadeSelect**: *{true/false} | default: false* | decide if parent selection should cascade to children in multiple selection
- **source**: *{JSON Data Array}* | takes source of combobox dropdown menu as a JSON array.
- **selected**: *{JSON Data Array}* | takes the list of ID's that corespond from the source.
- **collapse**: *{true/false} | default: false* | makes sub lists collapsed.

## Methods
- **getSelectedIds()**: Returns selected item(s) id list as array or null. *(i.e. [12, 5, 7], [7], null)*
- **getSelectedNames()**: Returns selected item(s) name list as array or null. *(i.e. ["Piegon", "Cat", "Horse"], ["Lion"], null)*
- **setSource()**: You can initialize ComboTree then set source after your JSON data is retrieved.
- **clearSelection()**: Clear selected items.
- **setSelection(selectionIdList)**: Set selected values of combotree by id array or single id parameter. If you want to clear previous selections please use *clearSelection()* before *setSelection()*.  *(i.e. ct1.setSelection([12, 5, 7]) | ct1.setSelection(5)*

## Events
- **onChange(callBackFunction)**: Triggers after selection changes.


## Usage

There should be an input element to apply and a JSON Data source.

	comboTree1 = $('#justAnInputBox').comboTree({
		source : SampleJSONData,
		isMultiple: true,
		cascadeSelect: true,
		selected: ['0']
	});

	// Array, One title/id, or False value return
	var selectedTitles = comboTree1.getSelectedItemsTitle();
	var selectedIds = comboTree1.getSelectedItemsId();
	
	// To remove plugin
	comboTree1.destroy();
	


## Source

Three parameter are needed: id, title and subs.

	var SampleJSONData = [
        {
            id: 0,
            title: 'Horse'
        }, {
            id: 1,
            title: 'Birds',
            subs: [
                {
                    id: 10,
                    title: 'Piegon'
                }, {
                    id: 11,
                    title: 'Parrot'
                }, {
                    id: 12,
                    title: 'Owl'
                }, {
                    id: 13,
                    title: 'Falcon'
                }
            ]
        }, {
            id: 2,
            title: 'Rabbit'
        }, {
            id: 3,
            title: 'Fox'
        }, {
            id: 5,
            title: 'Cats',
            subs: [
                {
                    id: 50,
                    title: 'Kitty'
                }, {
                    id: 51,
                    title: 'Bigs',
                    subs: [
                        {
                            id: 510,
                            title: 'Cheetah'
                        }, {
                            id: 511,
                            title: 'Jaguar'
                        }, {
                            id: 512,
                            title: 'Leopard'
                        }
                    ]
                }
            ]
        }, {
            id: 6,
            title: 'Fish'
        }
    ];



## You can donate to support me

https://www.blockchain.com/btc/address/15c5AxBVgNxkwaHSTBZMiCV5PL41DKe88v