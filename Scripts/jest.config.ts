/** @type {import("ts-jest/dist/types").InitialOptionsTsJest} */
module.exports = {
    testEnvironment: "jsdom",
    preset: 'ts-jest/presets/default-esm',
    globals: {
        'ts-jest': {
            tsconfig: 'tsconfig.json',
            useESM: true,
        },
    },
    transform: {
        '^.+\\.(js|jsx)$': 'babel-jest',
        '^.+\\.(ts|tsx)$': 'ts-jest',
    },
    transformIgnorePatterns: [
        '/node_modules/(エラーになっているライブラリ).+\\.ts',
    ],
};
