export const distanceGpsCoords = (segmentGps) =>
{
  const [[fromLat, fromLong], [toLat, toLong]] = segmentGps
  const radPerDeg = Math.PI/180
  const radiusMeters = 6371 * 1000
  const dlatRad = (toLat-fromLat) * radPerDeg
  const dlonRad = (toLong-fromLong) * radPerDeg
  const fromLatRad = fromLat * radPerDeg
  const toLatRad = toLat * radPerDeg
  const a = Math.sin(dlatRad/2)**2 + 
    Math.cos(fromLatRad) * Math.cos(toLatRad) * Math.sin(dlonRad/2)**2
  const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a))
  const r = Math.abs(radiusMeters * c)
  return r
}

const EQUAL_COORDS_METERS = 3

export const isEqualGpsCoords = (segmentGps) =>
{
  const r = (distanceGpsCoords(segmentGps) < EQUAL_COORDS_METERS)
  return r
}

export const intersectionGpsSegments = (segmentGpsOne, segmentGpsTwo) =>
{
  const [[fromLatOne, fromLongOne], [toLatOne, toLongOne]] = segmentGpsOne
  const [[fromLatTwo, fromLongTwo], [toLatTwo, toLongTwo]] = segmentGpsTwo
  const sphericalAccurancy = 0.000001
  const radPerDeg = Math.PI/180
  const fromLatOneRad = radPerDeg * fromLatOne
  const fromLongOneRad = radPerDeg * fromLongOne
  const toLatOneRad = radPerDeg * toLatOne
  const toLongOneRad = radPerDeg * toLongOne
  const fromLatTwoRad = radPerDeg * fromLatTwo
  const fromLongTwoRad = radPerDeg * fromLongTwo
  const toLatTwoRad = radPerDeg * toLatTwo
  const toLongTwoRad = radPerDeg * toLongTwo
  const sphericalFromOne = [
    Math.cos(fromLatOneRad) * Math.cos(fromLongOneRad),
    Math.cos(fromLatOneRad) * Math.sin(fromLongOneRad),
    Math.sin(fromLatOneRad)
  ]
  const sphericalToOne = [
    Math.cos(toLatOneRad) * Math.cos(toLongOneRad),
    Math.cos(toLatOneRad) * Math.sin(toLongOneRad),
    Math.sin(toLatOneRad)
  ]
  const sphericalFromTwo = [
    Math.cos(fromLatTwoRad) * Math.cos(fromLongTwoRad),
    Math.cos(fromLatTwoRad) * Math.sin(fromLongTwoRad),
    Math.sin(fromLatTwoRad)
  ]
  const sphericalToTwo = [
    Math.cos(toLatTwoRad) * Math.cos(toLongTwoRad),
    Math.cos(toLatTwoRad) * Math.sin(toLongTwoRad),
    Math.sin(toLatTwoRad)
  ]
  const normalOne = [
    sphericalFromOne[1]*sphericalToOne[2] -
    sphericalFromOne[2]*sphericalToOne[1],
    -(sphericalFromOne[0]*sphericalToOne[2] -
    sphericalFromOne[2]*sphericalToOne[0]),
    sphericalFromOne[0]*sphericalToOne[1] -
    sphericalFromOne[1]*sphericalToOne[0],
  ]
  const normalTwo = [
    sphericalFromTwo[1]*sphericalToTwo[2] -
    sphericalFromTwo[2]*sphericalToTwo[1],
    -(sphericalFromTwo[0]*sphericalToTwo[2] -
    sphericalFromTwo[2]*sphericalToTwo[0]),
    sphericalFromTwo[0]*sphericalToTwo[1] -
    sphericalFromTwo[1]*sphericalToTwo[0],
  ]
  const normalOfNormals = [
    normalOne[1]*normalTwo[2] -
    normalOne[2]*normalTwo[1],
    -(normalOne[0]*normalTwo[2] -
    normalOne[2]*normalTwo[0]),
    normalOne[0]*normalTwo[1] -
    normalOne[1]*normalTwo[0],
  ]
  const normalOfNormalsLength = Math.sqrt(
    normalOfNormals[0]*normalOfNormals[0]+
    normalOfNormals[1]*normalOfNormals[1]+
    normalOfNormals[2]*normalOfNormals[2]
  )
  const sphericalIntersectionFirst = 
  [
    normalOfNormals[0] / normalOfNormalsLength,
    normalOfNormals[1] / normalOfNormalsLength,
    normalOfNormals[2] / normalOfNormalsLength
  ]
  const sphericalIntersectionSecond = 
  [
    -normalOfNormals[0] / normalOfNormalsLength,
    -normalOfNormals[1] / normalOfNormalsLength,
    -normalOfNormals[2] / normalOfNormalsLength
  ]
  
  const angleSegmentOne = Math.acos((sphericalFromOne[0]*sphericalToOne[0]+
    sphericalFromOne[1]*sphericalToOne[1]+
    sphericalFromOne[2]*sphericalToOne[2]) /
    (Math.sqrt(sphericalFromOne[0]*sphericalFromOne[0]+
      sphericalFromOne[1]*sphericalFromOne[1]+
      sphericalFromOne[2]*sphericalFromOne[2]) *
    Math.sqrt(sphericalToOne[0]*sphericalToOne[0]+
      sphericalToOne[1]*sphericalToOne[1]+
      sphericalToOne[2]*sphericalToOne[2])
    ))
  const angleSegmentTwo = Math.acos((sphericalFromTwo[0]*sphericalToTwo[0]+
    sphericalFromTwo[1]*sphericalToTwo[1]+
    sphericalFromTwo[2]*sphericalToTwo[2]) /
    (Math.sqrt(sphericalFromTwo[0]*sphericalFromTwo[0]+
      sphericalFromTwo[1]*sphericalFromTwo[1]+
      sphericalFromTwo[2]*sphericalFromTwo[2]) *
    Math.sqrt(sphericalToTwo[0]*sphericalToTwo[0]+
      sphericalToTwo[1]*sphericalToTwo[1]+
      sphericalToTwo[2]*sphericalToTwo[2])
    ))

  const angleFromSegmentOneToFirstIntersection = Math.acos((sphericalFromOne[0]*sphericalIntersectionFirst[0]+
    sphericalFromOne[1]*sphericalIntersectionFirst[1]+
    sphericalFromOne[2]*sphericalIntersectionFirst[2]) /
    (Math.sqrt(sphericalFromOne[0]*sphericalFromOne[0]+
      sphericalFromOne[1]*sphericalFromOne[1]+
      sphericalFromOne[2]*sphericalFromOne[2]) *
    Math.sqrt(sphericalIntersectionFirst[0]*sphericalIntersectionFirst[0]+
      sphericalIntersectionFirst[1]*sphericalIntersectionFirst[1]+
      sphericalIntersectionFirst[2]*sphericalIntersectionFirst[2])
    ))
  const angleFromFirstIntersectionToSegmentOne = Math.acos((sphericalIntersectionFirst[0]*sphericalToOne[0]+
    sphericalIntersectionFirst[1]*sphericalToOne[1]+
    sphericalIntersectionFirst[2]*sphericalToOne[2]) /
    (Math.sqrt(sphericalIntersectionFirst[0]*sphericalIntersectionFirst[0]+
      sphericalIntersectionFirst[1]*sphericalIntersectionFirst[1]+
      sphericalIntersectionFirst[2]*sphericalIntersectionFirst[2]) *
    Math.sqrt(sphericalToOne[0]*sphericalToOne[0]+
      sphericalToOne[1]*sphericalToOne[1]+
      sphericalToOne[2]*sphericalToOne[2])
    ))
  const angleFromSegmentTwoToFirstIntersection = Math.acos((sphericalFromTwo[0]*sphericalIntersectionFirst[0]+
    sphericalFromTwo[1]*sphericalIntersectionFirst[1]+
    sphericalFromTwo[2]*sphericalIntersectionFirst[2]) /
    (Math.sqrt(sphericalFromTwo[0]*sphericalFromTwo[0]+
      sphericalFromTwo[1]*sphericalFromTwo[1]+
      sphericalFromTwo[2]*sphericalFromTwo[2]) *
    Math.sqrt(sphericalIntersectionFirst[0]*sphericalIntersectionFirst[0]+
      sphericalIntersectionFirst[1]*sphericalIntersectionFirst[1]+
      sphericalIntersectionFirst[2]*sphericalIntersectionFirst[2])
    ))
  const angleFromFirstIntersectionToSegmentTwo = Math.acos((sphericalIntersectionFirst[0]*sphericalToTwo[0]+
    sphericalIntersectionFirst[1]*sphericalToTwo[1]+
    sphericalIntersectionFirst[2]*sphericalToTwo[2]) /
    (Math.sqrt(sphericalIntersectionFirst[0]*sphericalIntersectionFirst[0]+
      sphericalIntersectionFirst[1]*sphericalIntersectionFirst[1]+
      sphericalIntersectionFirst[2]*sphericalIntersectionFirst[2]) *
    Math.sqrt(sphericalToTwo[0]*sphericalToTwo[0]+
      sphericalToTwo[1]*sphericalToTwo[1]+
      sphericalToTwo[2]*sphericalToTwo[2])
    ))

  const angleFromSegmentOneToSecondIntersection = Math.acos((sphericalFromOne[0]*sphericalIntersectionSecond[0]+
    sphericalFromOne[1]*sphericalIntersectionSecond[1]+
    sphericalFromOne[2]*sphericalIntersectionSecond[2]) /
    (Math.sqrt(sphericalFromOne[0]*sphericalFromOne[0]+
      sphericalFromOne[1]*sphericalFromOne[1]+
      sphericalFromOne[2]*sphericalFromOne[2]) *
    Math.sqrt(sphericalIntersectionSecond[0]*sphericalIntersectionSecond[0]+
      sphericalIntersectionSecond[1]*sphericalIntersectionSecond[1]+
      sphericalIntersectionSecond[2]*sphericalIntersectionSecond[2])
    ))
  const angleFromSecondIntersectionToSegmentOne = Math.acos((sphericalIntersectionSecond[0]*sphericalToOne[0]+
    sphericalIntersectionSecond[1]*sphericalToOne[1]+
    sphericalIntersectionSecond[2]*sphericalToOne[2]) /
    (Math.sqrt(sphericalIntersectionSecond[0]*sphericalIntersectionSecond[0]+
      sphericalIntersectionSecond[1]*sphericalIntersectionSecond[1]+
      sphericalIntersectionSecond[2]*sphericalIntersectionSecond[2]) *
    Math.sqrt(sphericalToOne[0]*sphericalToOne[0]+
      sphericalToOne[1]*sphericalToOne[1]+
      sphericalToOne[2]*sphericalToOne[2])
    ))
  const angleFromSegmentTwoToSecondIntersection = Math.acos((sphericalFromTwo[0]*sphericalIntersectionSecond[0]+
    sphericalFromTwo[1]*sphericalIntersectionSecond[1]+
    sphericalFromTwo[2]*sphericalIntersectionSecond[2]) /
    (Math.sqrt(sphericalFromTwo[0]*sphericalFromTwo[0]+
      sphericalFromTwo[1]*sphericalFromTwo[1]+
      sphericalFromTwo[2]*sphericalFromTwo[2]) *
    Math.sqrt(sphericalIntersectionSecond[0]*sphericalIntersectionSecond[0]+
      sphericalIntersectionSecond[1]*sphericalIntersectionSecond[1]+
      sphericalIntersectionSecond[2]*sphericalIntersectionSecond[2])
    ))
  const angleFromSecondIntersectionToSegmentTwo = Math.acos((sphericalIntersectionSecond[0]*sphericalToTwo[0]+
    sphericalIntersectionSecond[1]*sphericalToTwo[1]+
    sphericalIntersectionSecond[2]*sphericalToTwo[2]) /
    (Math.sqrt(sphericalIntersectionSecond[0]*sphericalIntersectionSecond[0]+
      sphericalIntersectionSecond[1]*sphericalIntersectionSecond[1]+
      sphericalIntersectionSecond[2]*sphericalIntersectionSecond[2]) *
    Math.sqrt(sphericalToTwo[0]*sphericalToTwo[0]+
      sphericalToTwo[1]*sphericalToTwo[1]+
      sphericalToTwo[2]*sphericalToTwo[2])
    ))

  let intersectionPositionGps = null
  if((Math.abs(angleFromSegmentOneToFirstIntersection + 
    angleFromFirstIntersectionToSegmentOne -
    angleSegmentOne) < sphericalAccurancy ) &&
    (Math.abs(angleFromSegmentTwoToFirstIntersection + 
    angleFromFirstIntersectionToSegmentTwo -
    angleSegmentTwo) < sphericalAccurancy ))
  {
    intersectionPositionGps = sphericalIntersectionFirst
  }
  else if((Math.abs(angleFromSegmentOneToSecondIntersection + 
    angleFromSecondIntersectionToSegmentOne -
    angleSegmentOne) < sphericalAccurancy ) &&
    (Math.abs(angleFromSegmentTwoToSecondIntersection + 
    angleFromSecondIntersectionToSegmentTwo -
    angleSegmentTwo) < sphericalAccurancy ))
  {
    intersectionPositionGps = sphericalIntersectionSecond
  }

  if(intersectionPositionGps != null)
  {
    intersectionPositionGps = [
      Math.asin(intersectionPositionGps[2]) / radPerDeg,
      Math.atan2((intersectionPositionGps[1] /
      Math.cos(Math.asin(intersectionPositionGps[2]))),
      (intersectionPositionGps[0] /
      Math.cos(Math.asin(intersectionPositionGps[2])))
      ) / radPerDeg
    ]
  }
    
  const r = intersectionPositionGps
  return r
}


export const crossGpsSegments = (segmentGpsOne, segmentGpsTwo) =>
{
  const r = (intersectionGpsSegments(segmentGpsOne, segmentGpsTwo) != null)
  return r
}


export const intermediateSegmentGpsPoint = (segmentGps, fraction) =>
{
  const [[fromLat, fromLong], [toLat, toLong]] = segmentGps
  const radPerDeg = Math.PI/180
  const alatRad = fromLat * radPerDeg
  const alonRad = fromLong * radPerDeg
  const blatRad = toLat * radPerDeg
  const blonRad = toLong * radPerDeg
  const dlon=blonRad-alonRad
  const alatRadSin = Math.sin(alatRad)
  const blatRadSin = Math.sin(blatRad)
  const alatRadCos = Math.cos(alatRad)
  const blatRadCos = Math.cos(blatRad)
  const dlonCos = Math.cos(dlon)
  const distance=Math.acos(alatRadSin*blatRadSin +
    alatRadCos*blatRadCos * dlonCos)
  const bearing=Math.atan2(Math.sin(dlon) * blatRadCos,
    alatRadCos*blatRadSin - alatRadSin*blatRadCos*dlonCos)
  const angularDistance=distance*fraction
  const angDistSin=Math.sin(angularDistance)
  const angDistCos=Math.cos(angularDistance)
  const xlatRad = Math.asin( alatRadSin*angDistCos +
    alatRadCos*angDistSin*Math.cos(bearing) )
  const xlonRad = alonRad + Math.atan2(
    Math.sin(bearing)*angDistSin*alatRadCos,
    angDistCos-alatRadSin*Math.sin(xlatRad))
  const xlat=xlatRad / radPerDeg
  const xlon=xlonRad / radPerDeg
  if(xlat>90)
  {
    xlat=90
  }
  if(xlat<-90)
  {
    xlat=-90
  }
  while(xlon>180)
  {
    xlon-=360;
  }
  while(xlon<=-180)
  {
    xlon+=360
  }

  const r = [xlat, xlon]
  return r
}

