using developwithpassion.specifications;
using developwithpassion.specifications.dsl.fieldswitching;
using developwithpassion.specifications.extensions;
using developwithpassion.specifications.rhinomocks;
using Machine.Specifications;

namespace developwithpassion.specification.specs
{
    public class FieldSwitcherSpecs
    {
        public class concern : Observes<ISwapValues, MemberTargetValueSwapper>
        {
            Establish c = () =>
            {
                original_value = "sdfsdfs";
                target = depends.on<MemberTarget>();
            };

            protected static MemberTarget target;
            protected static string original_value;
        }

        [Subject(typeof(ISwapValues))]
        public class when_constructed : concern
        {
            It should_use_the_target_to_get_the_original_value = () =>
                target.received(x => x.get_value());

            static string value_to_change_to;
        }

        [Subject(typeof(ISwapValues))]
        public class when_provided_the_value_to_change_to : concern
        {
            Establish c = () =>
            {
                value_to_change_to = "sdfsdf";
                target.setup(x => x.get_value()).Return(original_value);
            };

            Because b = () =>
                result = sut.to(value_to_change_to);

            It should_provide_the_pipeline_pair_that_can_do_the_switching = () =>
            {
                result.setup();
                target.received(x => x.change_value_to(value_to_change_to));
                result.teardown();
                target.received(x => x.change_value_to(original_value));
            };

            protected static ObservationPair result;
            protected static string value_to_change_to;
        }
    }
}