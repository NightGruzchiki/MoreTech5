import * as fs from "fs"
import * as path from "path"
import * as util from "util"
import * as url from "url"

const __dirname = url.fileURLToPath(new URL('.', import.meta.url));

export const readInputStreets = async() => {
  const streets = JSON.parse(await (util.promisify(fs.readFile))(
    path.join(__dirname, "..", "streets.json")
  ))
  return streets.streets
}

export const getStreetsObjs = async () => {
    const inputStreets = await readInputStreets()
    const resultStreets = []
    
    for(let i = 0; i<inputStreets.length; i++)
    {
        const {name:streetName, points: forwardStreetPoints} = inputStreets[i]
        const forwardStreet = {name:streetName,
            staticSegments: [],
            runSegments: []
        }
        for(let m = 0; m<forwardStreetPoints.length -1; m++)
        {
            const {staticSegments} = forwardStreet
            const currentSegmentGps = [forwardStreetPoints[m], forwardStreetPoints[m+1]]
            for(let k = 0; k<inputStreets.length; k++)
            {
                if(i == k) { continue }
                const {points: crossStreetPoints} = inputStreets[k]
                for(let n = 0; n<crossStreetPoints.length -1; n++)
                {
                    const crossSegmentGps = [crossStreetPoints[n], crossStreetPoints[n+1]]
                    const maybeIntersectionPositionGps = gps.intersectionGpsSegments(
                        currentSegmentGps, crossSegmentGps
                    )
                    if(maybeIntersectionPositionGps != null)
                    {
                        const [fromPositionGps, toPositionGps] = currentSegmentGps
                        staticSegments.push({
                            segment: [fromPositionGps, maybeIntersectionPositionGps],
                            intersections: [[k*2, n], [k*2 +1, n]]
                        })
                        staticSegments.push({
                            segment: [maybeIntersectionPositionGps, toPositionGps],
                            intersections: []
                        })
                    }
                    else
                    {
                        staticSegments.push({
                            segment: currentSegmentGps,
                            intersections: []
                        })
                    }
                }
            }
        }
        
        resultStreets.push(forwardStreet)

        const backwardStreet = {
          name:streetName,
            staticSegments: forwardStreet.staticSegments.slice()
              .reverse().map((forwaredSegmentDesc) => {
                const backwardSegmentDesc = { ...forwaredSegmentDesc,
                  intersections: forwaredSegmentDesc.intersections.slice(),
                  segment: [
                    forwaredSegmentDesc.segment[1],
                    forwaredSegmentDesc.segment[0]
                  ]
                }
                return backwardSegmentDesc
            }),
            runSegments: []
        }

        resultStreets.push(backwardStreet)
    }

    for(let i = 0; i< resultStreets.length; i++)
    {
      const currentStreet = resultStreets[i]
      for(const currentSegment of currentStreet.staticSegments)
      {
        for(let k = 0; k<currentSegment.intersections.length; k++)
        {
          const currentIntersection = currentSegment.intersections[k]
          const resultIntersection = []
          console.log(currentIntersection[0])
          resultIntersection[0] = resultStreets[currentIntersection[0]]
          resultIntersection[1] = resultIntersection[0].staticSegments[
            currentIntersection[1]
          ]
          currentSegment.intersections[k] = resultIntersection
        } 
      }
    }

    
    for(let i = 0; i<inputStreets.length; i++)
    {
        const {name:streetName, points: inputStreetPoints} = inputStreets[i]
        const forwardStreet = {name:streetName}
        const forwardStreetSegments = []
        for(let k = 0; k<streetPoints.length - 1; k++)
        {
            const currentSegmentGps = [streetPoints[k], streetPoints[k+1]]
            for(let m = 0; m<inputStreets.length; m++)
            {
                if(i == m) { continue }

            }

        }
        
        for(let k = 0; k<streetPoints.length - 1; k++)
        {
            const inputSegmentGps = [streetPoints[k], streetPoints[k+1]]
            const [[fromLat, fromLong], [toLat, toLong]] = inputSegmentGps
            const inputSegmentLength = distanceGpsCoords(inputSegmentGps)
            const outputSegmentsCount = Math.floor(inputSegmentLength / EQUAL_COORDS_METERS)

        }

  
    }
}