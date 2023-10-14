import * as server from "./server.mjs"

const main = async () => {
  try {
    await server.launchServer()
  }
  catch(err)
  {
    console.error(err)
  }
}

void main()
