import LinkButton from "./LinkButton";

const HeroSection = () => {
  return (
    <section className="text-center md:py-32 py-16">
      <div className="container mx-auto px-6">
        <h1 className="text-4xl font-semibold text-pastelDarkGreen mb-4">
          Free your money
        </h1>
        <p className="text-xl mb-10">
          Take control of your cash, smash those savings goals, and track your
          spending with ease.
        </p>
        <LinkButton
          to="/register"
          className="bg-pastelGreen text-white px-6 py-3 rounded-lg shadow-lg hover:bg-green-600 transition-colors duration-300"
        >
          Get started
        </LinkButton>
      </div>
    </section>
  );
};

export default HeroSection;
