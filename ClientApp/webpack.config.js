const path = require("path");
const webpack = require("webpack");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

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
        hot: true,
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
    devtool: "inline-source-map",
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
            // {
            //     test: /\.s[ac]ss$/i,
            //     use: [
            //         // Creates `style` nodes from JS strings
            //         "style-loader",
            //         // Translates CSS into CommonJS
            //         "css-loader",
            //         // Compiles Sass to CSS
            //         "sass-loader",
            //     ],
            // },
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
        new MiniCssExtractPlugin(),
    ],
    optimization: {
        minimizer: [
            // For webpack@5 you can use the `...` syntax to extend existing minimizers (i.e. `terser-webpack-plugin`), uncomment the next line
            `...`,
        ],
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