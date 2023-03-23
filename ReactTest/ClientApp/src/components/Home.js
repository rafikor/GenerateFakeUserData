import React, { Component } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component'

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            loading: true, regions: [], value: '',
            items: [], errorsPerRecord: 5, randomSeed:0
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleChangeSlider = this.handleChangeSlider.bind(this);
    }

    handleChange(event) {

        this.setState({ value: event.target.value }, () => { this.fetchData(0); });
        console.log(event.target.value);
        console.log(this.state.value);
        console.log('end');
    };

    handleChangeSlider(event) {
        if (event.target.value >= 0) {
            this.setState({ errorsPerRecord: event.target.value }, () => { this.fetchData(0); });
        }
        console.log(this.state.errorsPerRecord);
        console.log(this.state.errorsPerRecord);
        console.log(this.state.errorsPerRecord);
    };

    componentDidMount() {
        this.populateRegionsList();
        console.log(this.state.regions.length);

        
        
        
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
            headers: {
                selectedRegion: this.state.value, lengthGeneratedPrev: page == 0 ?0:this.state.items.length,
                errorsPerRecord: this.state.errorsPerRecord, randomSeed: this.state.randomSeed
            }
            //headers: { 'Content-Type': 'application/json' },
            //body: formData//JSON.stringify({ title: 'React POST Request Example' })
        };
        const response = await fetch('weatherforecast/GetForecast', requestOptions);
        const data = await response.json();
        for (let i = 0; i < data.length; i++) {
            newItems.push(data[i]);
        }
        //this.setState({ forecasts: data, loading: false });

        /*if (page === 100) {
            this.setState({ hasMore: false })
        }*/

        if (page == 0) {
            this.setState({ items: newItems })
        }
        else {
            this.setState({ items: [...this.state.items, ...newItems] })
        }
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

    /*static renderForecastsTable(forecasts) {
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
    }*/

    renderInfiniteScroll() {
        return (
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
            <table className='table table-striped' aria-labelledby="tabelLabelTable">
                <thead>
                    <tr>
                        <th>Index</th>
                        <th>Random identifier</th>
                            <th>Full name</th>
                            <th>Adress</th>
                            <th>Phone</th>
                    </tr>
                </thead>
                <tbody>
                    
                    {this.state.items.map((item, index) => 
                        <tr key={item.number}>
                            <td>{item.number}</td>
                            <td>{item.randomId}</td>
                            <td>{item.fullName}</td>
                            <td>{item.adress}</td>
                            <td>{item.phone}</td>
                        </tr>
                    )}
                    
                </tbody>
                </table>
            </InfiniteScroll>
        );
    }

    renderSlider(value, handleChangeSlider) {
        return (
            <div>
                <input
                    type="number"
                    placeholder="Your fav number"
                    value={value}
                    onChange={handleChangeSlider}
                  />
                <div className="slider-parent">
                    <input type="range" min="0" max="10" step="0.25"
                        onChange={handleChangeSlider}
                        value={value}

                    />
                    <div className="buble">
                        {value}
                    </div>
                </div>
            </div>
        );
    }

    render() {


        //const [value, setValue] = React.useState(this.regions[0]);
        //const [value, setValue] = React.useState(this.state.regions[0]);

        /*let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderForecastsTable(this.state.forecasts);*/

        let contentsRegions = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderDropdownRegions(this.state.regions, this.state.value, this.handleChange);

        let contentsInfiniteScroll = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderInfiniteScroll();

        let contentsRenderSlider = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderSlider(this.state.errorsPerRecord, this.handleChangeSlider);


        return (
            <div>
                <h1 id="tabelLabel" >Weather forecast</h1>
                <p>This component demonstrates fetching data from the server.</p>
                
                {contentsRegions}
                {contentsRenderSlider }
                {contentsInfiniteScroll}
            </div>
        );
    }

    /*async populateWeatherData() {
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
    }*/

    async populateRegionsList() {
        const response = await fetch('weatherforecast/GetRegions');
        const data = await response.json();
        this.setState({ regions: data, loading: false }, ()=>{
            if (this.state.regions.length > 0) {
                this.setState({ value: this.state.regions[0] }, () => { this.fetchData(0);});
            } });
    }
};



