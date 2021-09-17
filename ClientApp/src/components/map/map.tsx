import React, { Component } from 'react';
import Unity, { UnityContext } from 'react-unity-webgl';


interface GameState{
    progress: number,
    coordinates: {x: number, y: number}

}

const unityContext = new UnityContext({
    loaderUrl: "webGL/Build/webGL.loader.js",
    dataUrl: "webGL/Build/webGL.data",
    frameworkUrl: "webGL/Build/webGL.framework.js",
    codeUrl: "webGL/Build/webGL.wasm",

  });

export class Game extends Component<{}, GameState>{
    constructor(props: any){
        super(props);
        //super({});
        this.state = {
            progress: 0,
            coordinates: {x:0, y:0}
        };

        this.clickClickFunc = this.clickClickFunc.bind(this);
    }
    componentDidMount(){
        // EventListner на процесс загрузки
        unityContext.on("progress",  (progression) => {
            this.setState({
                progress: progression
            });
          });

        // EventListner на кастомный евент
        unityContext.on("ClickEvent", (x, y, z) =>{
            this.setState({
                coordinates: {x: x, y: y}
            })
        });
    }

    clickClickFunc(){
        // Дергаю публичную функцию TaskOnClick из скрипта на объекте GameHandler
        const objStr =  JSON.stringify(this.state.coordinates);
        console.log(objStr)
        unityContext.send("GameHandler", "TaskOnClick2", objStr);
    }

    render (){
        return (
            <div >
                <div >
                    { this.state.progress !== 1 &&
                        <p>Loading {this.state.progress * 100} percent...</p>
                    }
                    
                </div>
                <Unity 
                    unityContext={unityContext} 
                    style={{
                        height:"100%",
                        width: 1110
                    }}/>
                <button className="btn btn-primary" onClick={this.clickClickFunc}>ClickClick</button>
                
                <span>Координаты гексагона: {this.state.coordinates.x}, {this.state.coordinates.y}</span>
                
            </div>
        )
    }
}