import React, { Component } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component'

export class Home extends Component {
    static displayName = Home.name;
    static minRandomNumber = 0;
    static maxRandomNumber = 2147483647;
    

    constructor(props) {
        super(props);
        this.state = {
            loading: true, regions: [], selectedRegion: '',
            items: [], errorsPerRecord: 5, randomSeed:306412873, oldSeed:0
        };
        this.handleChangeRegion = this.handleChangeRegion.bind(this);
        this.handleChangeSlider = this.handleChangeSlider.bind(this);
        this.handleChangeRandomID = this.handleChangeRandomID.bind(this);
        this.handleGenerateRandomID = this.handleGenerateRandomID.bind(this);
        this.handleDownloadCSV = this.handleDownloadCSV.bind(this);
    }

    handleChangeRegion(event) {

        this.setState({ selectedRegion: event.target.value }, () => { this.fetchData(0); });
    };

    handleChangeSlider(event) {
        if (event.target.value >= 0) {
            this.setState({ errorsPerRecord: event.target.value }, () => { this.fetchData(0); });
        }
    };

    handleChangeRandomID(event) {
        this.setState({ randomSeed: event.target.value }, () => { this.fetchData(0); });
    };

    

    handleDownloadCSV(event) {
        const requestOptions = {
            method: 'POST',
            headers: {
                selectedRegion: this.state.selectedRegion, lengthToGenerate: this.state.items.length,
                errorsPerRecord: this.state.errorsPerRecord, randomSeed: this.state.randomSeed,
                "Content-Type": "text/csv"
            }
        };
        fetch("GenerateFakeUser/DownloadCsv", requestOptions)
            .then((responce) => {
                 let fileName = 'result.csv';
                     var t = responce.body.getReader();
                     t.read().then(({ done, value }) => {
                         const url = window.URL.createObjectURL(
                             new Blob([value], {
                                 type: 'text/csv',
                                 encoding: 'UTF-8'
                             })
                         );
                         const link = document.createElement('a');
                         link.href = url;
                         link.setAttribute('download', fileName);
                         document.body.appendChild(link);
                         link.click();
                         link.remove();
                     });
         });

    };

    
    randomNumberInRange(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

    handleGenerateRandomID(event) {
        let random = this.randomNumberInRange(Home.minRandomNumber, Home.maxRandomNumber);
        this.setState({ randomSeed: random }, () => { this.fetchData(0); });
    };

    componentDidMount() {
        this.populateRegionsList();
    };

    fetchData = async (page) => {
        const newItems = [];

        let currentRandomSeedToSend = this.state.randomSeed;
        if (page != 0) {
            currentRandomSeedToSend = this.state.oldSeed;
        }
        const requestOptions = {
            method: 'POST',
            headers: {
                selectedRegion: this.state.selectedRegion, lengthGeneratedPrev: page == 0 ?0:this.state.items.length,
                errorsPerRecord: this.state.errorsPerRecord, randomSeed: currentRandomSeedToSend
            }
        };
        const response = await fetch('GenerateFakeUser/GetNewData', requestOptions);
        const data = await response.json();
        for (let i = 0; i < data.length; i++) {
            newItems.push(data[i]);
        }
        this.setState({ oldSeed: data[data.length - 1].randomId });

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

                    Target region:&nbsp;

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

    renderInfiniteScroll() {
        return (
            <InfiniteScroll
                dataLength={this.state.items.length}
                next={this.fetchData}
                hasMore="true"
                loader={<h4>Loading...</h4>}
                endMessage={
                    <p style={{ textAlign: 'center' }}>
                        <b>You have seen it all</b>
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

    renderRandomSeedID(value) {
        return (
            <div>
                Random seed:&nbsp;
                <input
                    type="number"
                    value={value}
                    onChange={this.handleChangeRandomID}
                />
                    <button variant="contained" onClick={this.handleGenerateRandomID}>Update</button>
            </div>
        );
    }

    renderDownloadCSV() {
        return (
            <div>
                <button variant="contained" onClick={this.handleDownloadCSV}>Download CSV</button>
            </div>
        );
    }

    render() {

        let contentsRegions = this.state.loading
            ? <p><em>Loading...</em></p>
            : Home.renderDropdownRegions(this.state.regions, this.state.selectedRegion, this.handleChangeRegion);

        let contentsRandomNumber = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRandomSeedID(this.state.randomSeed);

        let contentsInfiniteScroll = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderInfiniteScroll();

        let contentsRenderSlider = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderSlider(this.state.errorsPerRecord, this.handleChangeSlider);

        let contentsRenderDownloadCSV = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderDownloadCSV();

        return (
            <div>
                <h1 id="tabelLabel" >Fake user data</h1>
                <p>This application demonstrates generation of fake user data for different regions.</p>
                
                {contentsRegions}
                {contentsRandomNumber}
                {contentsRenderSlider}
                {contentsRenderDownloadCSV}
                {contentsInfiniteScroll}
            </div>
        );
    }

    async populateRegionsList() {
        const response = await fetch('GenerateFakeUser/getregions');
        const data = await response.json();
        this.setState({ regions: data, loading: false }, ()=>{
            if (this.state.regions.length > 0) {
                this.setState({ selectedRegion: this.state.regions[0] }, () => { this.fetchData(0); });
            } });
    }
};
