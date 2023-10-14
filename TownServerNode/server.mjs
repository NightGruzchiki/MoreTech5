import express from "express"
import * as trafmodel from "./trafmodel.mjs"
import * as streets from "./streets.mjs"
import * as gps from "./gpscoords.mjs"

export const launchServer = () => {
    const server = express()
    server.get("/get_streets", async (req, res) => {

    })

    server.listen(3000)
}

