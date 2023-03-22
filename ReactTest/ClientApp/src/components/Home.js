import React, { Component } from 'react';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, regions: [], regionsOptions: [], value: '' };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {

        this.setState({ value: event.target.value });

    };

    componentDidMount() {
        this.populateRegionsList();
        this.populateWeatherData();
        /*console.log(this.regions[0]);

        for (let i = 0; i < this.regions.Length(); i++) {
            this.regionsOptions[i] = { Label: this.regions[i], Value: this.regions[i] }
        };*/
        
    };


    /*static renderDropdownRegions() {


        const handleChange = (event) => {

            setValue(event.target.value);

        };
        const Dropdown = ({ label, value, options, onChange }) => {

            return (

                <label>

                    {label}

                    <select value={value} onChange={onChange}>

                        {options.map((option) => (

                            <option value={option.value}>{option.label}</option>

                        ))}

                    </select>

                </label>

            );
        };
        return (
            <div>


                <label>

                    "Target region"

                    <select value="value">

                        {this.regionsOptions.map((option) => (

                            <option value={option.value}>{option.label}</option>

                        ))}

                    </select>

                </label>

            <p>We eat value!</p>

        </div>
        );
    }*/

    static renderDropdownRegions(regions, value, handleChange) {
         
        return (
            <div>


                <label>

                    Target region

                    <select value={value} onChange={handleChange}>

                        {regions.map((option) => (

                            <option value={option}>{option}</option>

                        ))}

                    </select>

                </label>

                <p>{value} is selected</p>

            </div>
            );
    }

    static renderForecastsTable(forecasts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date}>
                            <td>{forecast.date}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {


        //const [value, setValue] = React.useState(this.regions[0]);
        //const [value, setValue] = React.useState(this.state.regions[0]);

        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderForecastsTable(this.state.forecasts);

        let contentsRegions = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderDropdownRegions(this.state.regions, this.state.value, this.handleChange);

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contentsRegions}
                {contents}
            </div>
        );
    }

    async populateWeatherData() {
        const response = await fetch('weatherforecast/Get');
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }

    async populateRegionsList() {
        const response = await fetch('weatherforecast/GetRegions');
        const data = await response.json();
        this.setState({ regions: data, loading: false });
    }
};



