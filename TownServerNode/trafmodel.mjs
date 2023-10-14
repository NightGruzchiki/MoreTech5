import * as simulation from "simulation"

export const makeModelStreetSegment = () => {
  const model = new simulation.Model({
        timeStart: 0,
        timeLength: 1e4,
        timeUnits: "Seconds"
    });
    const streetCars = model.Stock({
    name: "Cars",
    initial: 3
  });
  const speedRate = model.Variable({
    name: "Speed Rate",
    value: 0.02
  });
  const streetTroughtput = model.Flow(null, streetCars, {
    rate: "[Cars] * [Speed Rate]"
  });
  model.Link(speedRate, streetTroughtput);
  
  return model
}

export const runStreetFlow = (model) => {
    model.Flow(s, i, {
        name: "Throught",
        rate: "[Cars] * 0.75"
      });
      
      model.Flow(i, r, {
        name: "ChangeDirection",
        rate: "[Cars] * 0.25"
      });

    return model
}

