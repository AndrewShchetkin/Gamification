const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

const config = {
    entry: {
        index: ["./src/index.tsx"],
    },
    output: {
        path: path.join(__dirname, "/dist/"),
        filename: "[name].bundle.js"
    },
    devServer: {
        historyApiFallback: true,
        hot: true,
        port: 8080,
        open: true,
        proxy: {
            '/api': {
                target: 'https://localhost:44312',
                secure: false,
                ws: true
            },
            '/hubs': {
                target: "https://localhost:44312",
                changeOrigin: true,
                secure: false,
                ws: true,
            },
        },
    },
    devtool: "source-map",
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: ["babel-loader"]
            },
            {
                test: /.s?css$/,
                use: [MiniCssExtractPlugin.loader, "css-loader", "sass-loader"],
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
        new CleanWebpackPlugin(),
        new webpack.ProvidePlugin({
            React: 'react',
            ReactDOM: 'react-dom'
        }),
        new HtmlWebpackPlugin({
            template: "./public/index.html",
            filename: "index.html",
            chunks: ['index'],
        }),
        new MiniCssExtractPlugin(),
    ],
    optimization: {
        minimizer: [],
    }
};


module.exports = (env, argv) => {
    config.mode = argv.mode;
    if (argv.mode === 'development'){

    }

    if (argv.mode === 'production') {
        
        config.optimization.minimize = true;
        config.optimization.minimizer.push(new TerserPlugin({
            test: /\.tsx?$/
        }));
    }

    return config;
}