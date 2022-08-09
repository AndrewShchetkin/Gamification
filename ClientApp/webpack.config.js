const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = {
    entry: {
        index: ["./src/index.tsx"],
    },
    mode: "development", //production
    devServer: {
        historyApiFallback: true,
        hot: true,
        open: true
     },
    output: {
        path: path.join(__dirname, "/dist/"),
        filename: "[name].bundle.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: ["babel-loader"]
            },
            {
                test: /\.css$/,
                use: ["style-loader", "css-loader", "postcss-loader"]
            },
            {
                test: /\.s[ac]ss$/i,
                use: [
                    // Creates `style` nodes from JS strings
                    "style-loader",
                    // Translates CSS into CommonJS
                    "css-loader",
                    // Compiles Sass to CSS
                    "sass-loader",
                ],
                // test: /\.s[ac]ss$/i,
                // use: ['style-loader', {
                //     loader: 'css-loader',
                //     options: {
                //         modules:{
                //             localIdentName: "[name]"
                //         }
                //     }
                // }, "sass-loader",]
            },
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/
            },
            {
                test: /\.html$/,
                use: "underscore-template-loader",
            },
            {
                test: /\.svg$/i,
                issuer: /\.[jt]sx?$/,
                use: ['@svgr/webpack'],
            }
        ]
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js']
    },
    plugins: [
        new webpack.ProvidePlugin({
            React: 'react',
            ReactDOM: 'react-dom'
        }),
        new HtmlWebpackPlugin({
            template: "./public/index.html",
            filename: "index.html",
            chunks: ['index'],
        }),

    ]
};