import React, { Component } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = { forecasts: [], loading: true, regions: [], regionsOptions: [], value: '', items: [] };
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {

        this.setState({ value: event.target.value }, this.populateWeatherData);
        console.log(event.target.value);
        console.log(this.state.value);
        console.log('end');
    };

    componentDidMount() {
        this.populateRegionsList();
        this.fetchData(1);
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

    fetchData = async (page) => {
        const newItems = [];
        console.log('kek');
        console.log(this.state.items.length);
        console.log(page);

        /*for (let i = 0; i < 100; i++) {
            newItems.push(i)
        }*/


        const requestOptions = {
            method: 'POST',
            headers: { title: (this.state.value != '' ? this.state.value : 'test'), lengthGeneratedPrev: this.state.items.length }
            //headers: { 'Content-Type': 'application/json' },
            //body: formData//JSON.stringify({ title: 'React POST Request Example' })
        };
        const response = await fetch('weatherforecast/GetForecast', requestOptions);
        const data = await response.json();
        for (let i = 0; i < data.length; i++) {
            newItems.push(data[i].summary);
        }
        //this.setState({ forecasts: data, loading: false });

        /*if (page === 100) {
            this.setState({ hasMore: false })
        }*/

        this.setState({ items: [...this.state.items, ...newItems] })
    }

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

    renderInfiniteScroll() {
        return (
            <div>
                <h1>Infinite Scroll</h1>
                <InfiniteScroll
                    dataLength={this.state.items.length}
                    next={this.fetchData}
                    hasMore="true"
                    loader={<h4>Loading...</h4>}
                    endMessage={
                        <p style={{ textAlign: 'center' }}>
                            <b>Yay! You have seen it all</b>
                        </p>
                    }
                >
                    {this.state.items.map((item, index) => (
                        <div key={index}>
                            {item}
                        </div>
                    ))}
                </InfiniteScroll>
            </div>
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

        let contentsInfiniteScroll = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderInfiniteScroll();
        

        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contentsRegions}
                {contents}
                {contentsInfiniteScroll}
            </div>
        );
    }

    async populateWeatherData() {
        let formData = new FormData();
        //formData.append({ title: "test" });
        console.log(this.state.value);

        const requestOptions = {
            method: 'POST',
            headers: { title: (this.state.value != '' ? this.state.value : 'test'), lengthGeneratedPrev :0}
            //headers: { 'Content-Type': 'application/json' },
            //body: formData//JSON.stringify({ title: 'React POST Request Example' })
        };
        const response = await fetch('weatherforecast/GetForecast', requestOptions);
        const data = await response.json();
        this.setState({ forecasts: data, loading: false });
    }

    async populateRegionsList() {
        const response = await fetch('weatherforecast/GetRegions');
        const data = await response.json();
        this.setState({ regions: data, loading: false });
    }
};



