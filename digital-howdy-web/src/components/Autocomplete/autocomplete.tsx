import React, { useState, MouseEvent, Fragment, KeyboardEvent, ChangeEvent, useEffect } from 'react';
import './autocomplete.css';

type AutoCompleteProps = {
    name: string,
    placeholder?: string,
    suggestions: string[],
    maxLength: number,
    onSelect: (item: string) => void,
    dataTestId?: string, 
    boolean:boolean
};

const AutoComplete = (props: AutoCompleteProps) => {
    const [activeSuggestion, setActiveSuggestion] = useState(0);
    const [filteredSuggestions, setFilteredSuggestions] = useState<string[]>([]);
    const [userInput, setUserInput] = useState('');
    const [boolean, setBoolean] = useState(false);
    const [pencil, setPencil] = useState(true);

    useEffect(() => {
        
    }, [filteredSuggestions, userInput, activeSuggestion])

    const onSuggestionClick = (event: MouseEvent<HTMLInputElement>) => {
        onSuggestionSelect(event.currentTarget.innerText);
    }

    const onSuggestionSelect = (name: string) => {
        setUserInput(name);
        setFilteredSuggestions([]);
        props.onSelect(name);
        setBoolean(true);
        setPencil(false);
    }

    const suggestionsComponent = (
        <div className="autocomplete-items">
            {filteredSuggestions.map((item, index) => {
                return (
                    <div key={index}
                         className={index === activeSuggestion ? "autocomplete-active" : ""}
                         onClick={onSuggestionClick}>
                        {item}
                    </div>
                )
            })}
        </div>
    )
    
    const onInputChange = (event: ChangeEvent<HTMLInputElement>) => {
        const {value, maxLength} = event.currentTarget;
        const input = value.slice(0, maxLength);
        const filtered = props.suggestions.filter(
            suggestion => suggestion.toLowerCase().indexOf(input.toLowerCase()) > -1
        );

        setUserInput(input);
        setFilteredSuggestions(filtered);
        
    }

    const onKeyDown = (event: KeyboardEvent<HTMLInputElement>) => {
        if (filteredSuggestions.length === 0) {
            return;
        }

        if (event.keyCode === 13) {
            onSuggestionSelect(filteredSuggestions[activeSuggestion]);
        } else if (event.keyCode === 38) {
            if (activeSuggestion === 0) {
                return;
            }

            setActiveSuggestion(activeSuggestion - 1);
        } else if (event.keyCode === 40) {
            if (activeSuggestion === filteredSuggestions.length - 1) {
                return;
            }

            setActiveSuggestion(activeSuggestion + 1);
        }
    }

    function editField() {
        setBoolean(false);
        setPencil(true);
        setUserInput("");
    }

    return(
        <Fragment>
            <div className="autocomplete">
                <div className="inner-addon right-addon">
                    <div onClick={editField} className="edit-button" hidden={pencil}>  
                        <i className="fas fa-pencil-alt"
                            hidden={pencil}
                            aria-hidden="false">
                        </i>
                    </div>
                    
                    <input
                        data-testid={props.dataTestId}
                        autoComplete="false"
                        className="autocomplete"
                        id="myInput"
                        type="text"
                        name={props.name}
                        placeholder={props.placeholder}
                        required
                        maxLength={props.maxLength}
                        onChange={onInputChange}
                        onKeyDown={onKeyDown}
                        value={userInput}
                        readOnly={boolean}
                        
                    />
                </div>
                {suggestionsComponent}
            </div>
        </Fragment>
    )
}

export default AutoComplete;