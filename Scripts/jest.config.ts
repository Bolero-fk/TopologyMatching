/** @type {import("ts-jest/dist/types").InitialOptionsTsJest} */
module.exports = {
    testEnvironment: "jsdom",
    preset: 'ts-jest/presets/default-esm',
    transform: {
        '^.+\\.(js|jsx)$': 'babel-jest',
        '^.+\\.(ts|tsx)$': ['ts-jest', {
            tsconfig: 'tsconfig.json',
            useESM: true,
        }],
    },
    coverageReporters: ["text", "html", "cobertura"],
    collectCoverage: true,
    collectCoverageFrom: [
        "dist/*.js"
    ],
};
